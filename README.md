# CORE_Practical_19

here i have two projects one is api and second  is frontend.

API is connected to database so you have to make database for that.

here also i used the code first approch so you have to chnage the connection string according to your pc.

now open the package manager console and select the project and run the following command.

<strong> Update-Database </strong>

now  you are good to go.

if you dont see the multiple starup projct like this than you have to do the following steps.

![image](https://github.com/AbhiSimform/CORE_Practical_18/assets/125336138/96b8f174-3b45-477e-ba75-21280262c58d)

now go to properties of solution file.

![image](https://github.com/AbhiSimform/CORE_Practical_18/assets/125336138/ec1d1700-b10a-4461-8d80-68b80c23eb47)

now you see the following window.

![image](https://github.com/AbhiSimform/CORE_Practical_18/assets/125336138/0dc5c0a2-d312-4599-b655-10081b8e5af6)

Now do the following changes

![image](https://github.com/AbhiSimform/CORE_Practical_18/assets/125336138/738fe929-00a3-4fdf-8202-07af091ffcea)

and do the apply now you can see the following thing:

![image](https://github.com/AbhiSimform/CORE_Practical_18/assets/125336138/9191197e-8eb7-4bf2-a809-0329eb66ad59)

 Note : only do the following steps if you do not have multiple startup objects

 Now : when you want to add Admin than Just Changed in below file

 API / Services / UserServices 

 inside that at 151 line of RegisterUser inside that You have to just chage to User Role.

 ![image](https://github.com/AbhiSimform/CORE_Practical_19/assets/125336138/ff310082-7cc5-4206-accf-ea47efa656b4)

and now Register User which are Register in "User" Role.
