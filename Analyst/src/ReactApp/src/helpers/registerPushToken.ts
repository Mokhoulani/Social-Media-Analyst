import * as SecureStore from 'expo-secure-store'
import { registerForPushNotificationsAsync } from '../notifications/notificationService'
import { DeviceActions } from '../store/device/actions'
import { store } from '../store/store'

const DEVICE_TOKEN_KEY = 'device_token'

/**
 * Registers the device's push notification token if needed.
 * @param deviceId The unique device ID
 */

export async function registerPushToken(deviceId: string): Promise<void> {
    try {
        const newToken = await registerForPushNotificationsAsync()
        if (!newToken) return

        const storedToken = await SecureStore.getItemAsync(DEVICE_TOKEN_KEY)

        if (__DEV__) {
            console.log('[DeviceManager] Stored token:', storedToken)
            console.log('[DeviceManager] New token:', newToken)
        }

        if (storedToken !== newToken) {
            // Token has changed or doesn't exist – sync with backend
            store.dispatch(
                DeviceActions.setDeviceRequest({
                    deviceToken: newToken,
                    deviceId,
                })
            )

            await SecureStore.setItemAsync(DEVICE_TOKEN_KEY, newToken, {
                keychainAccessible: SecureStore.WHEN_UNLOCKED_THIS_DEVICE_ONLY,
            })

            if (__DEV__)
                console.log(
                    '[DeviceManager] Updated push token in secure store'
                )
        }
    } catch (error) {
        console.error('[DeviceManager] Error registering device token:', error)
    }
}
