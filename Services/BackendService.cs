/*
    The backend service interacts
    with the backend
*/

using System.Reflection.Metadata.Ecma335;
using Combat_Critters_2._0.Models;

public static class BackendService
{
    public static async Task<bool> LoginAsync(UserCredentials credentials)
    {
        //Interact with the back end using the wrapper
        //return await WrapperClient.Login(credentials);
        return true;
    }


    public static async Task<bool> CreateAccountAsync(Profile credentials)
    {
        // Interact with the Java backend to create a new account
        //return await WrapperClient.CreateAccount(credentials);
        return true;
    }
}