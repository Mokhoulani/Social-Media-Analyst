import { z } from 'zod'

// User entity schema for validation and type inference
export const UserSchema = z.object({
    id: z.string().uuid(), // Optional: enforce UUID format for IDs
    firstName: z.string().min(1, { message: 'First name is required' }),
    lastName: z.string().min(1, { message: 'Last name is required' }),
    email: z.string().email({ message: 'Invalid email address' }),
})

// Derive TypeScript type from Zod schema
export type User = z.infer<typeof UserSchema>

// Optional: Create a function for creating a new user with validation
export function createUser(userData: unknown): User {
    return UserSchema.parse(userData)
}

// Optional: Create a function for partial updates
export const UserUpdateSchema = UserSchema.partial()
export type UserUpdate = z.infer<typeof UserUpdateSchema>

// Example of creating a user
export function exampleUserCreation() {
    try {
        const newUser = createUser({
            id: crypto.randomUUID(), // Using Web Crypto API for UUID
            firstName: 'John',
            lastName: 'Doe',
            email: 'john.doe@example.com',
            role: 'user',
            isActive: true,
        })
        return newUser
    } catch (error) {
        if (error instanceof z.ZodError) {
            console.error('Validation failed:', error.errors)
            throw error
        }
    }
}
