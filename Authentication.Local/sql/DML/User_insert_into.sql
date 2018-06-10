begin try
	begin transaction
		insert into [dbo].[user]
		values ('johndoe', '1234', 'John', 'Doe', 'john@doe.com', '2000-01-05'),
			   ('janedoe', '1234', 'Jane', 'Doe', 'jane.doe@email.com', '2007-03-06'),
			   ('dyrra', '1234', 'George', 'Dyrrachitis', 'george@dyrra.com', '1989-10-26'),
			   ('davedoe', '1234', 'Dave', 'Doe', 'dave.doe@email.com', '1987-09-09');
	commit transaction;
end try
begin catch
	rollback transaction;
	raiserror('Error while inserting', 0, 1) with nowait;
end catch