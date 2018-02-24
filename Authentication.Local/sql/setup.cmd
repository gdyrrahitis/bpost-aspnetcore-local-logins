:: Setup database schema and data

:: Setting up useful variables
set server="(LocalDB)\MSSQLLocalDB"
set databaseName="samples"

:: Setting up schema
sqlcmd -S %server% -i "./Tables/User.sql" -d %databaseName%
sqlcmd -S %server% -i "./Tables/UserClaims.sql" -d %databaseName%
sqlcmd -S %server% -i "./Tables/Meetup.sql" -d %databaseName%
sqlcmd -S %server% -i "./Tables/Meetup_Members_Rsvp.sql" -d %databaseName%
sqlcmd -S %server% -i "./Tables/Members.sql" -d %databaseName%

:: Provisioning with data
sqlcmd -S %server% -i "./DML/User_insert_into.sql" -d %databaseName%
sqlcmd -S %server% -i "./DML/UserClaims_insert_into.sql" -d %databaseName%
sqlcmd -S %server% -i "./DML/Meetup_insert_into.sql" -d %databaseName%
sqlcmd -S %server% -i "./DML/Meetup_Members_junction_insert_into.sql" -d %databaseName%
sqlcmd -S %server% -i "./DML/Members_insert_into.sql" -d %databaseName%
