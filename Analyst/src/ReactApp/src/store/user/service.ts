import { Observable } from 'rxjs'
import { User } from '../../entities/user'
import { APIClient } from '../../api/apiClient'

export interface GetUserPayload {
    accecsToken: string
}
export interface GetUserResponse {
    id: string
    firstName: string
    lastName: string
    email: string
}
export interface UserError {
    message: string
}

export const UserService = {
    /**
     * Gets the current user profile
     * @returns Observable with user data
     */
    getUser: (): Observable<User> => APIClient.get<User>('/User/get-user'),

    /**
     * Updates user profile information
     * @param userData Partial user data to update
     * @returns Observable with updated user data
     */
    updateUser: (userData: Partial<User>): Observable<User> =>
        APIClient.put<User>('/User/update', userData),

    /**
     * If you need the full response with headers, status, etc.
     * you can use the requestWithFullResponse method
     */
}
