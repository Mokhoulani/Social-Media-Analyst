import { z } from 'zod'

export const UserDeviceSchema = z.object({
    id: z.string().uuid().optional(),
    userId: z.string().uuid().optional(),
    deviceId: z.string().min(1, { message: 'Device ID is required' }),
    deviceToken: z.string().min(1, { message: 'Device token is required' }),
    createdOnUtc: z
        .string()
        .datetime({ message: 'Invalid UTC datetime' })
        .optional(),
})

export type UserDevice = z.infer<typeof UserDeviceSchema>

export function createUserDevice(data: unknown): UserDevice {
    return UserDeviceSchema.parse(data)
}

export const UserDeviceUpdateSchema = UserDeviceSchema.partial()

export type UserDeviceUpdate = z.infer<typeof UserDeviceUpdateSchema>

export function updateUserDevice(
    device: UserDevice,
    updates: Partial<UserDeviceUpdate>
): UserDevice {
    const updated = { ...device, ...updates }
    return UserDeviceSchema.parse(updated)
}

export function validateUserDevice(data: unknown): UserDevice {
    try {
        return UserDeviceSchema.parse(data)
    } catch (error) {
        if (error instanceof z.ZodError) {
            console.error('UserDevice validation failed:', error.errors)
            throw error
        }
        throw error
    }
}

export function validateUserDeviceUpdate(
    device: UserDevice,
    updates: unknown
): UserDeviceUpdate {
    try {
        return UserDeviceUpdateSchema.parse(updates)
    } catch (error) {
        if (error instanceof z.ZodError) {
            console.error('UserDevice update validation failed:', error.errors)
            throw error
        }
        throw error
    }
}
