Prerequiste: Need Docker            <br />
Windows: WSL2, Linux:Debian

To run this:-                       <br />
-docker compose up --build          <br />
-attach shell, inside container of aem-my-webapi-1: <br />

  1)dotnet new tool-manifest --force  <br />
  2)dotnet tool install --local dotnet-ef --version 7.0.7 <br />
  --for WSL/WSL2--
  3)dotnet clean

  4)dotnet ef migrations add InitialCreate <br />
  5)dotnet dotnet-ef database update <br />

To view the table,Phpmyadmin:<br /> http://localhost:8080/index.php , Username:user Password:admin123

login,getdata, view in json->      <br />
http://localhost/DataSave/Actual   <br />
http://localhost/DataSave/Dummy    <br />


login, getdata, save data -> saving process: Get <br />
http://localhost/DataSave/Actualx                <br />
http://localhost/DataSave/Dummyx                 <br />
