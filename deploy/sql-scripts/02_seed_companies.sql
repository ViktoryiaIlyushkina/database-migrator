START TRANSACTION;

-- 2. Обертка для безопасной вставки данных
DO $SEED$
BEGIN
    -- Проверяем, выполнялся ли уже этот блок данных
    IF NOT EXISTS(SELECT 1 FROM "__SeedDataHistory" WHERE "SeedDataId" = '20260407_SeedCompanies') THEN
        
        INSERT INTO "Companies" ("Name", "Email", "TelphoneNumber")
        VALUES 
        ('Global Tech Solutions', 'contact@globaltech.com', '+1-555-0101'),
        ('Green Energy Corp', 'info@greenenergy.io', '+1-555-0202'),
        ('Cyber Sentinel Inc', 'security@cybersentinel.net', '+1-555-0303'),
        ('Blue Horizon Logistics', 'support@bluehorizon.com', '+1-555-0404'),
        ('Apex Manufacturing', 'sales@apexmake.com', '+1-555-0505'),
        ('Quantum FinTech', 'hello@quantumfin.com', '+1-555-0606'),
        ('Starlight Media', 'media@starlight.com', '+1-555-0707'),
        ('Urban Dev Group', 'office@urbandev.org', '+1-555-0808'),
        ('Pioneer Health', 'care@pioneerhealth.com', '+1-555-0909'),
        ('EcoSystems Ltd', 'nature@ecosystems.com', '+1-555-1010');

        -- 3. Фиксируем выполнение сидинга в истории
        INSERT INTO "__SeedDataHistory" ("SeedDataId")
        VALUES ('20260407_SeedCompanies');

        RAISE NOTICE 'Companies seeded successfully.';
    ELSE
        RAISE NOTICE 'Companies already seeded, skipping.';
    END IF;
END $SEED$;

COMMIT;