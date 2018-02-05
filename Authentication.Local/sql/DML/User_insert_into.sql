begin try
	begin transaction
		insert into [dbo].[user]
		values ('johndoe', '1234', 'John', 'Doe', 'john@doe.com', '2000-01-05');
	commit transaction;
end try
begin catch
	rollback transaction;
	raiserror('Error while inserting', 0, 1) with nowait;
end catch