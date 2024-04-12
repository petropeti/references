import json

class EventFileManager:
    FILE_PATH = 'events.json'

    def read_events_from_file(self):
        try:
            with open(self.FILE_PATH, 'r') as file:
                events = json.load(file)
        except FileNotFoundError:
            events = []
        except Exception as e:
            print(f"Error reading events file: {e}")
            events = []

        return events
        
    def write_events_to_file(self, events):
        try:
            with open(self.FILE_PATH, 'w') as file:
                json.dump(events, file)
        except Exception as e:
            print(f"Error writing events to file: {e}")
