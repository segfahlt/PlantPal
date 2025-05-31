💡 Project Idea: “PlantPal: Jungle Ops”
It’s a .NET 9 WebAPI that helps you track and schedule your planting, fertilizing, watering, and harvesting tasks for your badass PR jungle yard.

🪴 Core Concept:
You have:

A list of plants or beds (e.g. “Marshmallow Patch”, “Turmeric Grove”, “Piper Zone”)

Each plant has a calendar of care events (e.g. fertilize every 14 days, water every 3 days, harvest in 120 days)

The API lets you:

Add/edit/remove plants

Add care plans (fertilizing, watering, harvesting)

Calculate next due date for each task

Return a list of what you should do today

Optionally: expose a GET /tasks/today endpoint you can hit from your phone on the porch with a cold rum & coke in hand

🧱 Core Features:
Models: Plant, CareEvent, TaskSchedule, maybe SoilZone

Services:

PlantService: Add/Edit/Delete plants

ScheduleService: Calculates what’s due today

ReminderService: (optional) spits out a daily task list

Controllers:

PlantController

ScheduleController

In-Memory Repo or JSON-based storage for starters

Swagger/OpenAPI enabled so you can test easily

Optional Upgrade Later: Hook into a Telegram bot or Twilio SMS to send you daily reminders like a digital farm gnome.

