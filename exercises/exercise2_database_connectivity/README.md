# Exercise 2 - Database Connectivity

Exercise 2 - Database Connectivity, is a simple application that presents the user with a list of book and/or movie series.  Users can add new series, edit or delete existing ones, and also print the series to a text file.

Of note, a custom form scaling routine was introduced to get the forms to scale down such that they would fit on my attached monitor.  This was particularly challenging, and probably needs a bit more tweaking to get just right.  Visual Studio does not support scaling all that well.

Also, images are stored in the database with the series themselves rather than storing keys into a separate image list.  This also proved quite challenging in that picture boxes apparently do not like to have their image contents read back to any kind of stream for conversion to a byte array to send to the database.  A workaround was found.