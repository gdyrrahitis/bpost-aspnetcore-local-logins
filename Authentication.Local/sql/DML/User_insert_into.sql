begin try
	begin transaction
		insert into [dbo].[user]
		select [m].username, [m].[password], [m].firstname, [m].surname, [m].email, [m].dateofbirth, [m].updatedOn from (
			select 'johndoe' [username], '1234' [password], 'John' [firstname], 'Doe' [surname], 'john@doe.com' [email], '2000-01-05' [dateofbirth], getdate() [updatedOn]
			union all
			select'janedoe' [username], '1234' [password], 'Jane' [firstname], 'Doe' [surname], 'jane@doe.com' [email], '1992-06-15' [dateofbirth], getdate() [updatedOn]
		) as [m]

	commit transaction;
end try
begin catch
	rollback transaction;
	raiserror('Error while inserting', 0, 1) with nowait;
end catch