import * as Device from 'expo-device'
import * as SecureStore from 'expo-secure-store'
import { useEffect, useState } from 'react'
import { v4 as uuidv4 } from 'uuid'

const DEVICE_ID_KEY = 'unique_device_id'

export const useDeviceId = () => {
    const [deviceId, setDeviceId] = useState<string | null>(null)

    useEffect(() => {
        const loadDeviceId = async () => {
            try {
                const storedId = await SecureStore.getItemAsync(DEVICE_ID_KEY)
                if (storedId) {
                    setDeviceId(storedId)
                    return
                }

                const generatedId = Device.osInternalBuildId ?? uuidv4()

                await SecureStore.setItemAsync(DEVICE_ID_KEY, generatedId)
                setDeviceId(generatedId)
            } catch (error) {
                console.warn('Failed to load or generate device ID:', error)
            }
        }

        loadDeviceId()
    }, [])

    return deviceId
}
