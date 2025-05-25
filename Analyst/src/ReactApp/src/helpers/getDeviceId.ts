import * as Device from 'expo-device'
import * as SecureStore from 'expo-secure-store'
import { v4 as uuidv4 } from 'uuid'

const DEVICE_ID_KEY = 'unique_device_id'

/**
 * Retrieves or generates a unique device ID.
 * @returns The device ID as a string.
 */
export async function getDeviceId(): Promise<string> {
    try {
        const storedId = await SecureStore.getItemAsync(DEVICE_ID_KEY)
        if (storedId) {
            return storedId
        }

        const generatedId = Device.osInternalBuildId ?? uuidv4()
        await SecureStore.setItemAsync(DEVICE_ID_KEY, generatedId)
        return generatedId
    } catch (error) {
        console.warn('Failed to retrieve or generate device ID:', error)
        throw error
    }
}
