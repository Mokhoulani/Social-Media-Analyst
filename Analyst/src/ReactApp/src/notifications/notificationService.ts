import * as Device from 'expo-device'
import * as Notifications from 'expo-notifications'

export async function registerForPushNotificationsAsync(): Promise<
    string | null
> {
    let token

    if (Device.isDevice) {
        const { status: existingStatus } =
            await Notifications.getPermissionsAsync()
        let finalStatus = existingStatus

        if (existingStatus !== 'granted') {
            const { status } = await Notifications.requestPermissionsAsync()
            finalStatus = status
        }

        if (finalStatus !== 'granted') {
            return null
        }

        token = (await Notifications.getExpoPushTokenAsync()).data
    } else {
        alert('Must use physical device for Push Notifications')
        return null
    }

    return token
}
