import * as SecureStore from 'expo-secure-store'
import { registerForPushNotificationsAsync } from '../notifications/notificationService'
import { DeviceActions } from '../store/device/actions'
import { store } from '../store/store'

const DEVICE_TOKEN_KEY = 'device_token'

/**
 * Registers the device's push notification token if needed.
 * @param isAuthenticated Whether the user is authenticated
 * @param deviceId The unique device ID
 */
export async function registerPushToken(
    isAuthenticated: boolean,
    deviceId: string
): Promise<void> {
    if (!isAuthenticated) return

    try {
        const newToken = await registerForPushNotificationsAsync()
        if (!newToken) return

        const storedToken = await SecureStore.getItemAsync(DEVICE_TOKEN_KEY)

        if (storedToken !== newToken) {
            store.dispatch(
                DeviceActions.setDeviceRequest({
                    deviceToken: newToken,
                    deviceId,
                })
            )

            await SecureStore.setItemAsync(DEVICE_TOKEN_KEY, newToken)
        }
    } catch (error) {
        console.error('Error registering device token:', error)
    }
}
