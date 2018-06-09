begin try
	begin transaction
		insert into [dbo].[members]
		values ('johndoe', 'Admin', 0),
			   ('janedoe', 'Author', 0),
			   ('dyrra', 'Moderator', 1);
	commit transaction;
end try
begin catch
	rollback transaction;
	raiserror('Error while inserting', 0, 1) with nowait;
end catch