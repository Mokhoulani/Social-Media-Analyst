import { z } from 'zod'

export const UserSchema = z.object({
    id: z.string().uuid(),
    firstName: z.string().min(1, { message: 'First name is required' }),
    lastName: z.string().min(1, { message: 'Last name is required' }),
    email: z.string().email({ message: 'Invalid email address' }),
})

export type User = z.infer<typeof UserSchema>

export function createUser(userData: unknown): User {
    return UserSchema.parse(userData)
}

export const UserUpdateSchema = UserSchema.partial()

export type UserUpdate = z.infer<typeof UserUpdateSchema>

export function updateUser(user: User, updates: Partial<UserUpdate>): User {
    const updatedUser = { ...user, ...updates }
    return UserSchema.parse(updatedUser)
}

export function validateUser(user: unknown): User {
    try {
        return UserSchema.parse(user)
    } catch (error) {
        if (error instanceof z.ZodError) {
            console.error('Validation failed:', error.errors)
            throw error
        }
        throw error
    }
}

export function validateUserUpdate(user: User, updates: unknown): UserUpdate {
    try {
        return UserUpdateSchema.parse(updates)
    } catch (error) {
        if (error instanceof z.ZodError) {
            console.error('Validation failed:', error.errors)
            throw error
        }
        throw error
    }
}
