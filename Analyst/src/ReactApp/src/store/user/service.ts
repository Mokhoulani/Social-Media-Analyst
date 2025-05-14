import { Observable } from 'rxjs'
import { User } from '../../entities/user'
import { BaseApi } from '../../service/api/base-api'

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
    getUser: (): Observable<User> => BaseApi.get<User>('/User/get-user'),

    /**
     * Updates user profile information
     * @param userData Partial user data to update
     * @returns Observable with updated user data
     */
    updateUser: (userData: Partial<User>): Observable<User> =>
        BaseApi.put<User>('/User/update', userData),

    /**
     * If you need the full response with headers, status, etc.
     * you can use the requestWithFullResponse method
     */
}
