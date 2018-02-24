begin try
	begin transaction
		insert into [dbo].[roles]
		values (1, 'Member'),
			   (2, 'Co-Founder'),
			   (3, 'Founder');
	commit transaction;
end try
begin catch
	rollback transaction;
	raiserror('Error while inserting', 0, 1) with nowait;
end catch