from faker import Faker
import pytz
import random
import uuid
fake = Faker('ro_RO')

# Open a file to write the SQL statements
with open("populate_emergency_events.sql", "w") as f:
    # Generate and write INSERT statements in batches of 1000
    for i in range(1, 21):
        values = []
        # Generate 1000 records at a time
        for j in range(i, i + 21):
            id = uuid.uuid4()
            description = fake.text()
            location = fake.address()
            latitude =random.uniform(43.5, 48.0)
            longitude = random.uniform(20.0, 29.7)
            severity = random.randint(1, 4)
            status = random.randint(1, 3)
            utc_datetime = fake.date_time_between(start_date='-1y', end_date='now', tzinfo=pytz.utc)
            utc_datetime_str = utc_datetime.strftime('%Y-%m-%d %H:%M:%S')
            reported_at = utc_datetime_str
            reported_by = '377c33c8-8a28-4d97-a450-297a79ce3894'
            updated_at = utc_datetime_str
            type = random.randint(1, 9)
            values.append(f"('{id}', '{description}', '{location}', '{latitude}', '{longitude}', '{severity}', '{status}', '{reported_at}', '{reported_by}', '{updated_at}', '{type}')")
        # Write a batch of 1000 records to the file
        f.write(f"INSERT INTO \"EmergencyEvents\" (\"Id\", \"Description\", \"Location\", \"Latitude\", \"Longitude\", \"Severity\", \"Status\", \"ReportedAt\", \"ReportedBy\", \"UpdatedAt\", \"Type\") VALUES {', '.join(values)};\n")