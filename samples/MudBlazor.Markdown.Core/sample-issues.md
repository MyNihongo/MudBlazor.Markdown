# [Nested text](https://github.com/MyNihongo/MudBlazor.Markdown/issues/233)
> To prevent the warning message regarding the deprecation of the `mysql_native_password` plugin from being logged, you have a couple of options:
> 
> Option 1: Update User Authentication Method:
> 
> 1. Connect to your MySQL server using a MySQL client, such as the `mysql` command-line tool:
>    ```bash
>    mysql -u username -p
>    ```
> 
> 2. Once connected, run the following command to alter the user's authentication method:
>    ```sql
>    ALTER USER 'username'@'hostname' IDENTIFIED WITH caching_sha2_password;
>    ```
>    Replace `'username'` with the actual username and `'hostname'` with the appropriate hostname or IP address. If you want to update for all users, replace `'username'@'hostname'` with `'*'@'%'`.
> 
> 3. Repeat this process for each user on your MySQL server.

# [Table in list](https://github.com/MyNihongo/MudBlazor.Markdown/issues/144)
## Prereq
The following requirements must be met outside this Terraform code in advance.

1. Created a new resource group..
2. Created a new AAD group using naming convention..
3. Created a new AAD group using naming convention..
4. Created a separate storage account..
5. Created a new app registration..

  |Permission | Type | Granted Through|
  |--|--|--|
  |User.Read | Delegated | Admin consent|
  | User.ReadBasic.All |Delegated| Admin consent|
  |User.Read.All|Delegated| Admin consent|
  |Directory.Read.Alll|Delegated| Admin consent|
  |Directory.AccessAsUser.All|Delegated| Admin consent|
  |User.Invite.All|Application| Admin consent|
  |Directory.Read.All|Application| Admin consent|
  |User.Read.All|Application| Admin consent|
  - Granted permissions User.Invite.All and GroupMember.ReadWrite.All. Admin consent has been granted

6. Create a new app registration..
7. Created the key vault..
8. Created secrets..

# [Table (not indented) in list](https://github.com/MyNihongo/MudBlazor.Markdown/issues/145)
## Prereq
The following requirements must be met outside this Terraform code in advance.

1. Created a new resource group..
2. Created a new AAD group using naming convention..
3. Created a new AAD group using naming convention..
4. Created a separate storage account..
5. Created a new app registration..

|Permission | Type | Granted Through|
|--|--|--|
|User.Read | Delegated | Admin consent|
| User.ReadBasic.All |Delegated| Admin consent|
|User.Read.All|Delegated| Admin consent|
|Directory.Read.Alll|Delegated| Admin consent|
|Directory.AccessAsUser.All|Delegated| Admin consent|
|User.Invite.All|Application| Admin consent|
|Directory.Read.All|Application| Admin consent|
|User.Read.All|Application| Admin consent|
- Granted permissions User.Invite.All and GroupMember.ReadWrite.All. Admin consent has been granted

6. Create a new app registration..
7. Created the key vault..
8. Created secrets..
