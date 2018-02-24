begin try
	begin transaction
		insert into [dbo].[members]
		values ('johndoe'),
			   ('janedoe'),
			   ('dyrra');
	commit transaction;
end try
begin catch
	rollback transaction;
	raiserror('Error while inserting', 0, 1) with nowait;
end catch