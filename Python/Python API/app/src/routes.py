from fastapi import APIRouter, HTTPException
from typing import List
from .models import Event
from .models import Event
from .file_storage import EventFileManager

router = APIRouter()


@router.get("/events", response_model=List[Event])
async def get_all_events():
    event_manager = EventFileManager()
    events = event_manager.read_events_from_file()
    return events


@router.get("/events/filter", response_model=List[Event])
async def get_events_by_filter(date: str = None, organizer: str = None, status: str = None, event_type: str = None):
    event_manager = EventFileManager()
    events = event_manager.read_events_from_file()
    filtered_events = []
    for event in events:
        if (date==None or event['date'] == date) and\
        (organizer==None or event['organizer']['name'] == organizer) and\
        (status==None or event['status'] == status) and\
        (event_type==None or event['type'] == event_type):
            filtered_events.append(event)
    
    return filtered_events


@router.get("/events/{event_id}", response_model=Event)
async def get_event_by_id(event_id: int):
    event_manager = EventFileManager()
    events = event_manager.read_events_from_file()
    for event in events:
        if event['id'] == event_id:
            return event
    return HTTPException(status_code=404, detail="Event not found")


@router.post("/events", response_model=Event)
async def create_event(event: Event):
    event_manager = EventFileManager()
    events = event_manager.read_events_from_file()
    for e in events:
        if e['id'] == event.id:
            return HTTPException(status_code=400, detail="Event ID already exists")
    events.append(event.dict())
    event_manager.write_events_to_file(events)
    return event


@router.put("/events/{event_id}", response_model=Event)
async def update_event(event_id: int, event: Event):
    event_manager = EventFileManager()
    events = event_manager.read_events_from_file()
    for e in events:
        if e['id'] == event_id:
            e.update(event.dict())
            event_manager.write_events_to_file(events)
            return e
    return HTTPException(status_code=404, detail="Event not found")


@router.delete("/events/{event_id}")
async def delete_event(event_id: int):
    event_manager = EventFileManager()
    events = event_manager.read_events_from_file()
    for e in events:
        if e['id'] == event_id:
            events.remove(e)
            event_manager.write_events_to_file(events)
            return {"message": "Event deleted successfully"}
    return {"message": "Event not found"}


@router.get("/events/joiners/multiple-meetings")
async def get_joiners_multiple_meetings():
    pass
