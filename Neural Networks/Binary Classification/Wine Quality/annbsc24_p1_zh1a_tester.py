
#
# Automatic tests for Midterm software test (ZH) #1, version A (8apr2024) in ELTE IK, ANN BSc course part1, 2024 spring
#
# Authors: Viktor Varga
#

import warnings
import copy as copy_module
import os
import urllib
import numpy as np

import torch
import torch.nn as nn

class Tester:

    '''
    Member fields:

        TESTS_DICT: dict{test_name - str: test function - Callable}

        dataset_content: None OR str; dataset loaded into a string

        # STORED DATA FROM PREVIOUS TESTS
        
        self.dataset_noisy: ndarray(n_samples, n_features) fl32
        self.dataset: ndarray(n_samples, n_features) fl32; no NaNs

        # RESULT OF LASTEST PREVIOUS TEST RUNS
        test_results: dict{test_name - str: success - bool}

    '''

    def __init__(self):

        self.TESTS_DICT = {
              'dataset_load': self.__test_dataset_load,
              'dataset_fill_missing': self.__test_dataset_fill_missing,
              'dataset_split': self.__test_dataset_split,
              'bincl_iter': self.__test_bincl_model_iter,
              'bincl_model_architecture': self.__test_bincl_model_architecture,
              'bincl_model_learning': self.__test_bincl_model_learning}

        self.test_results = {k: False for k in self.TESTS_DICT.keys()}
        self.dataset_content = None

        self.dataset_noisy = None
        self.dataset = None

    def get_dataset_content(self):
        '''
        Sets self.dataset_content.
        Returns:
            dataset_content: str
        '''
        fpath = "winequality-white24_zh1a.csv"
        with open(fpath, 'r') as f:
            self.dataset_content = f.read()

        return self.dataset_content

    def test(self, test_name, *args):
        '''
        Parameters:
            test_name: str
            *args: varargs; the arguments for the selected test
        '''
        if test_name not in self.TESTS_DICT:
            self.print_tester_error_msg(f"Tester error: Invalid test name: {test_name}", "<pre-test>")

        self.test_results[test_name] = False
        test_func = self.TESTS_DICT[test_name]
        test_func(*args)
        self.test_results[test_name] = True    # only executed if no assert happened during test

    def print_tester_error_msg(self, msg, test_name):
        print(f"\nTester error in test '{test_name}' ->  {msg}\n")

    def print_all_tests_successful(self):
        if all(list(self.test_results.values())):
            print("\nTester: All tests were successful.")


    # TESTS

    def __test_dataset_load(self, *args):
        '''
        Expected parameters:
            dataset_noisy: ndarray(n_samples, n_features) of float32
        '''
        try:
            assert len(args) == 1, "Tester error: __test_dataset_load() expects 1 parameters: dataset_noisy. "
            dataset_noisy, = args

            assert type(dataset_noisy) == np.ndarray, "Tester: 'dataset_noisy' must be a numpy array."
            assert dataset_noisy.dtype == np.float32, "Tester: 'dataset_noisy' array must have float32 data type."
            expected_shape = (4898, 16)
            assert dataset_noisy.shape == expected_shape, f"Tester: 'dataset_noisy' array must have a shape of {expected_shape}."

            nanmask = np.isnan(dataset_noisy)
            nanidxs = np.argwhere(nanmask)

            expected_nancount = 1958
            expected_nancols = [5, 6]
            expected_nanpos50_60 = [[110, 6], [112, 6], [113, 5], [113, 6], [119, 6], [120, 5], [121, 5], [126, 5], [126, 6], [129, 5]]

            assert np.count_nonzero(nanmask) == expected_nancount, "Tester: Incorrect NaN count in 'dataset_noisy' array."
            assert np.all(np.isin(nanidxs[:,1], expected_nancols)), "Tester: NaN values found in incorrect columns of 'dataset_noisy' array."
            assert np.array_equal(nanidxs[50:60], expected_nanpos50_60), "Tester: Incorrect NaN value positions in 'dataset_noisy' array."

            print("Tester: Dataset loading OK")
        except AssertionError as assert_msg:
            self.print_tester_error_msg(assert_msg, 'dataset_load')
        except Exception as exception_msg:
            self.print_tester_error_msg(exception_msg, 'dataset_load')

        self.dataset_noisy = np.copy(dataset_noisy)

    def __test_dataset_fill_missing(self, *args):
        '''
        Expected parameters:
            dataset: ndarray(n_samples, n_features) of float32
        '''
        try:
            assert len(args) == 1, "Tester error: __test_dataset_fill_missing() expects 1 parameters: dataset. "
            dataset, = args
            assert self.dataset_noisy is not None, "Tester error: Run tester for task 'A' first."

            assert type(dataset) == np.ndarray, "Tester: 'dataset' must be a numpy array."
            assert dataset.dtype == np.float32, "Tester: 'dataset' array must have float32 data type."
            expected_shape = (4898, 16)
            assert dataset.shape == expected_shape, f"Tester: 'dataset' array must have a shape of {expected_shape}."
            assert not np.any(np.isnan(dataset)), "Tester: There should be no NaN values in the 'dataset' array."
            nanmask = np.isnan(self.dataset_noisy)
            assert np.allclose(np.nansum(self.dataset_noisy), np.sum(dataset[~nanmask]), atol=1e-2), "Tester: Non-NaN values from the 'dataset_noisy' array should be unchanged in the 'dataset' array."

            print("Tester: Dataset fill missing OK")
        except AssertionError as assert_msg:
            self.print_tester_error_msg(assert_msg, 'dataset_fill_missing')
        except Exception as exception_msg:
            self.print_tester_error_msg(exception_msg, 'dataset_fill_missing')

        self.dataset = np.copy(dataset)

    def __test_dataset_split(self, *args):
        '''
        Expected parameters:
            dataset_split_train, dataset_split_val, dataset_split_test: ndarray(n_split_samples, n_features) of float32
        '''
        try:
            assert len(args) == 3, "Tester error: __test_dataset_split() expects 3 parameters: " +\
                                        "dataset_split_train, dataset_split_val, dataset_split_test. "
            assert self.dataset is not None, "Tester error: Run tester for task 'A' & 'B' first."
            dataset_split_train, dataset_split_val, dataset_split_test = args

            dataset_split_arrs = [dataset_split_train, dataset_split_val, dataset_split_test]
            dataset_split_sizes = {'dataset_split_train': 3428, 'dataset_split_val': 735, 'dataset_split_test': 735}
            for dataset_split_arr, dataset_split_size_entry in zip(dataset_split_arrs, dataset_split_sizes.items()):
              dataset_split_name, dataset_split_size = dataset_split_size_entry
              assert type(dataset_split_arr) == np.ndarray, f"Tester: '{dataset_split_name}' must be a numpy array."
              assert dataset_split_arr.dtype == np.float32, f"Tester: '{dataset_split_name}' array must have float32 data type."

              assert dataset_split_arr.ndim == 2, f"Tester: '{dataset_split_name}' array has an incorrect shape."
              assert dataset_split_arr.shape[1] == self.dataset.shape[1], f"Tester: '{dataset_split_name}' array has an incorrect shape."
              assert np.fabs(dataset_split_arr.shape[0] - dataset_split_size) < 4, f"Tester: '{dataset_split_name}' array has an incorrect shape."

            expected_shape = (4898, 16)
            dataset_split_concat = np.concatenate([dataset_split_train, dataset_split_val, dataset_split_test], axis=0)
            assert dataset_split_concat.shape == dataset_split_concat.shape, "Tester: concatenated dataset splits do not match original 'dataset' array shape."
            assert not np.array_equal(dataset_split_concat, self.dataset), "Tester: Samples in dataset were not shuffled randomly."
            sample_sums_concat = np.sort(np.sum(dataset_split_concat, axis=1))
            sample_sums_orig = np.sort(np.sum(self.dataset, axis=1))
            assert np.allclose(sample_sums_concat, sample_sums_orig, atol=1e-2), "Tester: Samples might have been shuffled incorrectly (?)."

            print("Tester: Dataset split OK")
        except AssertionError as assert_msg:
            self.print_tester_error_msg(assert_msg, 'dataset_split')
        except Exception as exception_msg:
            self.print_tester_error_msg(exception_msg, 'dataset_split')


    def __test_bincl_model_iter(self, *args):
        '''
        Expected parameters:
            dataloader_bincl_train, dataloader_bincl_val, dataloader_bincl_test: ndarray(n_split_samples, n_features) of float32
        '''
        try:
            assert len(args) == 3, "Tester error: __test_bincl_model_iter() expects 3 parameters: " +\
                                        "dataloader_bincl_train, dataloader_bincl_val, dataloader_bincl_test. "
            dataloader_bincl_train, dataloader_bincl_val, dataloader_bincl_test = args

            dataloader_iters = [dataloader_bincl_train, dataloader_bincl_val, dataloader_bincl_test]
            dataloader_names = ['dataloader_bincl_train', 'dataloader_bincl_val', 'dataloader_bincl_test']
            for dataloader_iter, dataloader_name in zip(dataloader_iters, dataloader_names):
              dataloader_iter = copy_module.copy(dataloader_iter)
              for r in dataloader_iter:   # torch DataLoader implements only __getitem__(), but not __iter__(), so next() does not work
                break
              assert len(r) == 2, f"Tester: '{dataloader_name}' must return two tensors in each iteration."
              r0, r1 = r
              assert type(r0) == type(r1) == torch.Tensor, f"Tester: '{dataloader_name}' must return tensors."
              assert r0.dtype == r1.dtype == torch.float32, f"Tester: '{dataloader_name}' must return tensors with float32 data type."
              assert r0.ndim == r1.ndim == 2, f"Tester: Tensors returned by '{dataloader_name}' have incorrect shape."
              batch_size, n_feat = r0.shape
              assert r1.shape[0] == batch_size, f"Tester: Tensors returned by '{dataloader_name}' have inconsistent shape."
              assert n_feat == 11, f"Tester: Input feature tensor returned by '{dataloader_name}' have incorrect shape."
              assert r1.shape[1] == 1, f"Tester: Label tensor returned by '{dataloader_name}' have incorrect shape."

            print("Tester: Dataset iterators for binary classification task OK")
        except AssertionError as assert_msg:
            self.print_tester_error_msg(assert_msg, 'bincl_iter')
        except Exception as exception_msg:
            self.print_tester_error_msg(exception_msg, 'bincl_iter')


    def __test_bincl_model_architecture(self, *args):
        '''
        Expected parameters:
            bincl_model: PyTorch nn.Module
        '''
        try:
            assert len(args) == 1, "Tester error: __test_bincl_model_architecture() expects 1 parameter: bincl_model "
            bincl_model, = args
            
            assert isinstance(bincl_model, torch.nn.Module), f"Tester: The 'bincl_model' neural network model must be of a subtype of torch.nn.Module."
            n_params = sum([p.numel() for p in bincl_model.parameters(recurse=True) if p.requires_grad])
            assert n_params == (11*20 + 20) + (20*10 + 10) + (10*1 + 1), f"Tester: Incorrect structure in 'bincl_model' network (incorrect parameter count)."

            print("Tester: Binary classification model architecture OK")
        except AssertionError as assert_msg:
            self.print_tester_error_msg(assert_msg, 'bincl_model_architecture')
        except Exception as exception_msg:
            self.print_tester_error_msg(exception_msg, 'bincl_model_architecture')

    def __test_bincl_model_learning(self, *args):
        '''
        Expected parameters:
            test_bce, test_acc: float
        '''
        try:
            assert len(args) == 2, "Tester error: __test_bincl_model_learning() expects 2 parameter: test_bce, test_acc. "
            test_bce, test_acc = args
            assert 0.15 < test_bce < 0.4, "Tester: A well trained binary classification model should provide a test loss (BCE) between 0.15 and 0.4."
            assert 0.8 < test_acc < 0.94, "Tester: A well trained binary classification model should provide a test accuracy between 0.8 and 0.94."

            print("Tester: Binary classification model learning OK")
            self.print_all_tests_successful()
        except AssertionError as assert_msg:
            self.print_tester_error_msg(assert_msg, 'bincl_model_learning')
        except Exception as exception_msg:
            self.print_tester_error_msg(exception_msg, 'bincl_model_learning')
    #