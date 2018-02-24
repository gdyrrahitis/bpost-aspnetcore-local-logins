begin try
	begin transaction
		insert into [dbo].[meetup]
		values ('C# Dublin', getdate()),
			   ('Angular Meetup Group', getdate());
	commit transaction;
end try
begin catch
	rollback transaction;
	raiserror('Error while inserting', 0, 1) with nowait;
end catch