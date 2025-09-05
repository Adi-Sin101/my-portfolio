@echo off
echo Setting up Portfolio Images...
echo.

REM Create Uploads directory if it doesn't exist
if not exist "Uploads" mkdir Uploads

echo Please copy the following files from your VS Code project:
echo.
echo Profile Images:
echo - Copy Adiba_pic.jpg to this directory
echo.
echo Experience Images (copy to this directory):
echo - hult1.jpg
echo - SheStem.jpg  
echo - casecrackfinalist.jpg
echo - KBEC_member.jpg
echo.
echo Project Images (copy to this directory):
echo - portfilo.png
echo - pawpal.jpeg
echo - fitnesstracker.jpeg
echo - casecrack.jpg
echo.
echo Resume:
echo - Adiba_CV.pdf
echo.
echo After copying all files, run the SQL script in Database/portfolio-schema.sql
echo to set up your database tables.
echo.
echo Then you can access:
echo - Admin Panel: Login.aspx (username: admin, password: admin123)
echo - Public Portfolio: Home.aspx
echo.
pause