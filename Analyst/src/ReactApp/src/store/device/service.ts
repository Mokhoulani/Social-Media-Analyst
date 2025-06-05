import { Observable } from 'rxjs'
import { APIClient } from '../../api/apiClient'
import { UserDevicePyload } from './actions'

export interface UserDeviceResponse {
    id: string
    deviceId: string
    deviceToken: string
    CreatedOnUtc: Date
}

export const DeviceService = {
    /**
     * Creates or updates a user device
     * @param deviceData Device data to create or update
     * @returns Observable with user device data
     */

    createOrUpdateDevice: (
        deviceData: UserDevicePyload
    ): Observable<UserDeviceResponse> =>
        APIClient.post<UserDeviceResponse>('/User/device', deviceData),
}
