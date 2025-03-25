

![alt text](/screenshot.JPG)



The application as it is provides safe and efficient directory browsing and file fetching from an external source.
It also comes with built-in user management features, role management and enables setting custom access rules for individual files. 

For future development I'd like to add:

- Multimedia features e.g. embedded Image and video viewing, maybe thumbnails ✓

- Multiple remote sources ✓

- Option to upload files ✓

- Making file fetching even more secure with an generated "access token" or some sort like that 

- Maybe later adding Nextcloud-like features  



To Initialize the Database:

>dotnet tool install --global dotnet-ef

>dotnet ef migrations add Initial

>dotnet ef database update
