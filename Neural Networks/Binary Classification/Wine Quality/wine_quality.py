import copy
import numpy as np
import torch
import torch.nn as nn
from torch.utils.data import Dataset, DataLoader
import matplotlib.pyplot as plt
from annbsc24_p1_zh1a_tester import Tester

#Az adatbázis betöltése

tester = Tester()
content = tester.get_dataset_content()
print("Number of characters in dataset:", len(content))
print(content[:500])
lines = content.split('\n')
words = [line.split(',') for line in lines]
attr_names = words[0]
data = words[1:-1]
data = [[float(item) if item!='-' else float(np.nan) for item in rec] for rec in data]
dataset_noisy = np.array(data, dtype=np.float32)

#A hiányzó adat kezelése

attr_names = np.array(attr_names, dtype=np.string_)
nanmask = np.isnan(dataset_noisy)
dataset_flags = dataset_noisy[:, np.any(nanmask, axis=0)]
dataset_flags = (~np.isnan(dataset_flags)).astype(np.bool_)
dataset = np.where(np.isnan(dataset_noisy), 0, dataset_noisy)
dataset[:, 5:7] = np.where(dataset_flags, dataset[:, 5:7], np.median(dataset[:, 5:7], axis=0))

#Tanító-, validációs- és teszthalmaz szétválasztása

np.random.shuffle(dataset)

train_count = int(dataset.shape[0] * 0.7)
val_count = int(dataset.shape[0] * 0.15)
test_count = int(dataset.shape[0] * 0.15)

dataset_split_train = dataset[:train_count, :]
dataset_split_val = dataset[train_count:(train_count + val_count), :]
dataset_split_test = dataset[(train_count + val_count):, :]

#Az adat-iterátorok létrehozása a bináris klasszifikációs feladathoz

#Rossz minőségű borok kategória, #0 kategóriaindex:
#Azok a borok, ahol az ötből legalább négy borszakértő jelölt meg 4-es, vagy kisebb pontszámot.

#Jó minőségű borok kategória, #1 kategóriaindex:
#Azok a borok, ahol az öt borszakértő által adott pontszámok átlaga legalább 7.

class WineClassificationDataset(Dataset):
    def __init__(self, features, target):
        self.features = torch.tensor(features, dtype=torch.float32)
        self.target = torch.tensor(target, dtype=torch.float32)

    def __len__(self):
        return len(self.features)

    def __getitem__(self, idx):
        return self.features[idx], self.target[idx]

x_train_cl = dataset_split_train[:, :-5]
y_train_cl = dataset_split_train[:, -5:]
x_val_cl = dataset_split_val[:, :-5]
y_val_cl = dataset_split_val[:, -5:]
x_test_cl = dataset_split_test[:, :-5]
y_test_cl = dataset_split_test[:, -5:]

bad_wine_mask_train = np.sum(np.where(y_train_cl <= 4, 1, 0).astype(np.float32), axis=1)[:, None]>=4
good_wine_mask_train = np.mean(y_train_cl, axis=1)[:, None]>=7
wine_cl_train = np.where(bad_wine_mask_train, 0, np.where(good_wine_mask_train, 1, -1))
not_medium_wine_mask_train = np.where(wine_cl_train != -1, 1, 0).astype(np.bool_)
y_train_cl = wine_cl_train[np.where(not_medium_wine_mask_train)[0]]
x_train_cl = x_train_cl[np.where(not_medium_wine_mask_train)[0]]


bad_wine_mask_val = np.sum(np.where(y_val_cl <= 4, 1, 0).astype(np.float32), axis=1)[:, None]>=4
good_wine_mask_val = np.mean(y_val_cl, axis=1)[:, None]>=7
wine_cl_val = np.where(bad_wine_mask_val, 0, np.where(good_wine_mask_val, 1, -1))
not_medium_wine_mask_val = np.where(wine_cl_val != -1, 1, 0).astype(np.bool_)
y_val_cl = wine_cl_val[np.where(not_medium_wine_mask_val)[0]]
x_val_cl = x_val_cl[np.where(not_medium_wine_mask_val)[0]]


bad_wine_mask_test = np.sum(np.where(y_test_cl <= 4, 1, 0).astype(np.float32), axis=1)[:, None]>=4
good_wine_mask_test = np.mean(y_test_cl, axis=1)[:, None]>=7
wine_cl_test = np.where(bad_wine_mask_test, 0, np.where(good_wine_mask_test, 1, -1))
not_medium_wine_mask_test = np.where(wine_cl_test != -1, 1, 0).astype(np.bool_)
y_test_cl = wine_cl_test[np.where(not_medium_wine_mask_test)[0]]
x_test_cl = x_test_cl[np.where(not_medium_wine_mask_test)[0]]


train_dataset_cl = WineClassificationDataset(x_train_cl, y_train_cl)
val_dataset_cl = WineClassificationDataset(x_val_cl, y_val_cl)
test_dataset_cl = WineClassificationDataset(x_test_cl, y_test_cl)

dataloader_bincl_train = DataLoader(train_dataset_cl, batch_size=32, shuffle=True)
dataloader_bincl_val = DataLoader(val_dataset_cl, batch_size=32, shuffle=True)
dataloader_bincl_test = DataLoader(test_dataset_cl, batch_size=32, shuffle=False)

#A neuronháló definiálása

class BinClassificationModel(nn.Module):
  def __init__(self, input_dim):
        super().__init__()
        self.linear_relu_layers = nn.Sequential(
            nn.Linear(input_dim, 20),
            nn.ReLU(),
            nn.Dropout(p=0.1),
            nn.Linear(20, 10),
            nn.ReLU(),
            nn.Dropout(p=0.1),
            nn.Linear(10, 1)
        )
        self.sigmoid = nn.Sigmoid()

  def forward(self, x):
      out = self.linear_relu_layers(x)
      out = self.sigmoid(out)
      return out

features_count = x_train_cl.shape[1]
bincl_model = BinClassificationModel(features_count)

#A háló betanítása bináris klasszifikációs feladatra

class Callback():
  def __init__(self):
    pass

  def on_train_end(self, avg_train_loss):
    pass

  def on_val_end(self, avg_val_loss):
    pass

  def on_test_end(self, avg_test_loss):
    pass

  def on_epoch_end(self, trainer):
    pass

  def on_train_prediction(self, prediction, label):
    pass

  def on_val_prediction(self, prediction, label):
    pass

  def on_test_prediction(self, prediction, label):
    pass

  def on_stop(self, results):
    pass

class EarlyStoppingCallback(Callback):
  def __init__(self, delta=0.01, patience=10, save_path=None, model=None):
    self.delta = delta
    self.patience = patience
    self.best_loss = np.inf
    self.no_improvement_count = 0
    self.save_path = save_path
    self.model = model

  def on_val_end(self, avg_val_loss):
    if avg_val_loss + self.delta < self.best_loss:
      self.best_loss = avg_val_loss
      self.no_improvement_count = 0
      if self.save_path is not None and self.model is not None:
        torch.save(self.model.state_dict(), self.save_path)
    else:
      self.no_improvement_count += 1

  def on_epoch_end(self, trainer):
    if self.no_improvement_count >= self.patience:
      print('Stopped by early stopping.')
      print('Best validation loss: ', self.best_loss.item())
      trainer.stop = True

class MetricsCallback(Callback):
  def __init__(self, metrics_to_track):
    self.metrics_to_track = metrics_to_track
    self.__reset()

  def __reset(self):
    self.metrics_temp = dict()
    self.metrics = dict()

  def __add_prediction(self, prediction, label, prefix):
    for metric_name, metric_fn in self.metrics_to_track.items():
      key = f'{prefix}_{metric_name}'
      if key not in self.metrics:
        self.metrics[key] = []
        self.metrics_temp[key] = []

      self.metrics_temp[key].append(metric_fn(prediction, label).reshape(-1))

  def __aggregate_metrics(self, prefix):
    for key in self.metrics_to_track.keys():
      self.metrics[f'{prefix}_{key}'].append(torch.cat(self.metrics_temp[f'{prefix}_{key}']).mean().item())
      self.metrics_temp[f'{prefix}_{key}'] = []

  def on_train_prediction(self, prediction, label):
    self.__add_prediction(prediction, label, 'train')

  def on_val_prediction(self, prediction, label):
    self.__add_prediction(prediction, label, 'val')

  def on_test_prediction(self, prediction, label):
    self.__add_prediction(prediction, label, 'test')

  def on_train_end(self, avg_train_loss):
    self.__aggregate_metrics('train')

  def on_val_end(self, avg_val_loss):
    self.__aggregate_metrics('val')

  def on_test_end(self, avg_test_loss):
    self.__aggregate_metrics('test')

  def on_stop(self, results):
    metrics = {key: np.array(val) for key,val in self.metrics.items()}
    results.update(metrics)
    self.__reset()

class Trainer():
  def __init__(self, model, train_dataloader, val_dataloader, optimizer, loss_fn, n_epochs=10, callbacks=[]):
    self.model = model
    self.train_dataloader = train_dataloader
    self.val_dataloader = val_dataloader
    self.optimizer = optimizer
    self.loss_fn = loss_fn
    self.n_epochs = n_epochs
    self.stop = False
    self.callbacks = callbacks
    self.train_losses = []
    self.val_losses = []

  def __train(self, dataloader):
    self.model.train()
    train_epoch_losses = []

    for x, y in dataloader:
      self.optimizer.zero_grad()
      y_hat = self.model(x)
      loss = self.loss_fn(y_hat, y)
      [callback.on_train_prediction(y_hat.detach(), y) for callback in self.callbacks]
      train_epoch_losses.append(loss)
      loss.backward()
      self.optimizer.step()

    train_epoch_losses = torch.tensor(train_epoch_losses)
    avg_epoch_loss = train_epoch_losses.mean()

    [callback.on_train_end(avg_epoch_loss) for callback in self.callbacks]

    return avg_epoch_loss

  def __eval(self, dataloader, test=False):
    self.model.eval()
    epoch_losses = []

    for x, y in dataloader:
      with torch.no_grad():
        y_hat = self.model(x)

      loss = self.loss_fn(y_hat, y)
      if test:
        [callback.on_test_prediction(y_hat.detach(), y) for callback in self.callbacks]
      else:
        [callback.on_val_prediction(y_hat.detach(), y) for callback in self.callbacks]

      epoch_losses.append(loss)

    epoch_losses = torch.tensor(epoch_losses)
    avg_epoch_loss = epoch_losses.mean()

    if test:
      [callback.on_test_end(avg_epoch_loss) for callback in self.callbacks]
    else:
      [callback.on_val_end(avg_epoch_loss) for callback in self.callbacks]

    return avg_epoch_loss

  def train(self):
    for epoch in range(self.n_epochs):
      train_loss = self.__train(self.train_dataloader)
      self.train_losses.append(train_loss)
      val_loss = self.__eval(self.val_dataloader, test=False)
      self.val_losses.append(val_loss)

      print(f'Tranining {epoch+1}/{self.n_epochs} done, training loss: {train_loss}, validation loss: {val_loss}')

      [callback.on_epoch_end(self) for callback in self.callbacks]

      if self.stop:
        break

    results = {
        'train_loss': self.train_losses,
        'val_loss': self.val_losses
    }

    [callback.on_stop(results) for callback in self.callbacks]

    return results

  def test(self, test_dataloader):
    test_loss = self.__eval(test_dataloader, test=True)

    results = {
        'test_loss': test_loss
    }

    [callback.on_stop(results) for callback in self.callbacks]

    return results

#Betanítás

loss_fn = nn.BCELoss()
learning_rate = 0.001
optimizer = torch.optim.Adam(bincl_model.parameters(), lr=learning_rate)

def accuracy(pred, label):
  return torch.mean((torch.round(pred) == label).to(torch.float32))

metrics = {
    'Accuracy': accuracy
}

earlystopping_callback = EarlyStoppingCallback(delta=0.01, patience=20, save_path='best_model.pth')
metrics_callback = MetricsCallback(metrics)
callbacks = [earlystopping_callback, metrics_callback]
trainer = Trainer(bincl_model, dataloader_bincl_train, dataloader_bincl_val, optimizer, loss_fn, n_epochs=400, callbacks=callbacks)
results = trainer.train()

#Grafikon

plt.plot(results['train_Accuracy'], label='Training')
plt.plot(results['val_Accuracy'], label='Validation')
plt.xlabel('Epochs')
plt.ylabel('Accuracy')
plt.legend()
plt.show()

#Tesztelés

bincl_model.eval()
accuracy = []
prediction_label_pairs = []
for x, y in dataloader_bincl_test:
    with torch.no_grad():
      y_hat = bincl_model(x)
    prediction = torch.round(y_hat)
    prediction_label_pairs.append(torch.concat((prediction, y), dim=-1))
    accuracy.append((prediction == y).to(torch.float32))

test_acc = torch.cat(accuracy).mean().item()
test_bce = trainer.test(dataloader_bincl_test)['test_loss'].item()
print('Test accuracy: ', test_acc)
print('Test loss: ', test_bce)

print('A few predictions from the test dataset: ')
for pred, y in prediction_label_pairs[0]:
  print('Pred: ', pred.item(), '\tTrue label: ', y.item())

#Konfúziós mátrix

from sklearn.metrics import confusion_matrix
import seaborn as sns

prediction_label_pairs = torch.cat(prediction_label_pairs, 0)
conf = confusion_matrix(prediction_label_pairs[:,1], prediction_label_pairs[:,0])

plt.figure(figsize=(5, 4))
sns.heatmap(conf, annot=True, cmap='coolwarm', fmt=".2f", linewidths=.5)
plt.title('Confusion matrix')
plt.xlabel('Prediction')
plt.ylabel('True label')
plt.show()
