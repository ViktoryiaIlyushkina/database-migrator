START TRANSACTION;

DO $SEED$
BEGIN
    -- Проверяем, не запускался ли этот конкретный сид ранее
    IF NOT EXISTS(SELECT 1 FROM "__SeedDataHistory" WHERE "SeedDataId" = '20260407_SeedEmployees') THEN
        
        INSERT INTO "Employees" ("FullName", "CompanyId")
        VALUES 
        ('Alice Johnson', (SELECT "Id" FROM "Companies" WHERE "Name" = 'Global Tech Solutions' LIMIT 1)),
        ('Bob Smith', (SELECT "Id" FROM "Companies" WHERE "Name" = 'Green Energy Corp' LIMIT 1)),
        ('Charlie Davis', (SELECT "Id" FROM "Companies" WHERE "Name" = 'Cyber Sentinel Inc' LIMIT 1)),
        ('Diana Prince', (SELECT "Id" FROM "Companies" WHERE "Name" = 'Blue Horizon Logistics' LIMIT 1)),
        ('Evan Wright', (SELECT "Id" FROM "Companies" WHERE "Name" = 'Apex Manufacturing' LIMIT 1)),
        ('Fiona Glenanne', (SELECT "Id" FROM "Companies" WHERE "Name" = 'Quantum FinTech' LIMIT 1)),
        ('George Miller', (SELECT "Id" FROM "Companies" WHERE "Name" = 'Starlight Media' LIMIT 1)),
        ('Hannah Abbott', (SELECT "Id" FROM "Companies" WHERE "Name" = 'Urban Dev Group' LIMIT 1)),
        ('Ian Curtis', (SELECT "Id" FROM "Companies" WHERE "Name" = 'Pioneer Health' LIMIT 1)),
        ('Julia Roberts', (SELECT "Id" FROM "Companies" WHERE "Name" = 'EcoSystems Ltd' LIMIT 1));

        -- Ставим отметку о выполнении
        INSERT INTO "__SeedDataHistory" ("SeedDataId")
        VALUES ('20260407_SeedEmployees');

    END IF;
END $SEED$;

COMMIT;