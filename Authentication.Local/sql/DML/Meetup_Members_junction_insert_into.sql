begin try
	begin transaction
		insert into [dbo].[MeetupMembersRsvp]
		values (1, 1, 1, 'Member'),
			   (1, 2, 1, 'Co-Founder'),
			   (1, 3, 1, 'Founder'),
			   (2, 3, 1, 'Member'),
			   (2, 1, 1, 'Founder'),
			   (2, 2, 0, 'Member');
	commit transaction;
end try
begin catch
	rollback transaction;
	raiserror('Error while inserting', 0, 1) with nowait;
end catch