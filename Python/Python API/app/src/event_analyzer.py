from .models import Event
from collections import defaultdict

class EventAnalyzer:
    def get_joiners_multiple_meetings_method(self, events: list[Event]):
        joiners_count = defaultdict(int)
        for event in events:
            for joiner in event['joiners']:
                joiners_count[joiner['name']] += 1
        return [joiner for joiner, count in joiners_count.items() if count >= 2]
    