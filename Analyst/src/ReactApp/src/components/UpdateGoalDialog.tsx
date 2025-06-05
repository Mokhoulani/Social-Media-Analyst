import { zodResolver } from '@hookform/resolvers/zod'
import React, { useCallback, useEffect } from 'react'
import { Controller, useForm } from 'react-hook-form'
import {
    ActivityIndicator,
    Alert,
    SafeAreaView,
    Text,
    TextInput,
    TouchableOpacity,
    View,
} from 'react-native'
import { Dropdown } from 'react-native-element-dropdown'
import Modal from 'react-native-modal'
import { useSelector } from 'react-redux'
import { SocialMediaPlatform } from '../entities/SocialMediaPlatform'
import { UserUsageGoal, UserUsageGoalSchema } from '../entities/UserUsageGoal'
import { userSelectors } from '../store/user/selectors'
import { useStyles } from '../themes/useStyles'

interface Props {
    visible: boolean
    onClose: () => void
    onSubmit: (data: UserUsageGoal) => void
    goal: UserUsageGoal | null
    platforms: SocialMediaPlatform[] | null
}

export const UpdateGoalDialog: React.FC<Props> = ({
    visible,
    onClose,
    onSubmit,
    goal,
    platforms,
}) => {
    const user = useSelector(userSelectors.selectUser)
    const styles = useStyles()

    const {
        control,
        handleSubmit,
        reset,
        formState: { errors, isSubmitting },
        setValue,
    } = useForm<UserUsageGoal>({
        resolver: zodResolver(UserUsageGoalSchema),
        defaultValues: {
            id: goal?.id,
            userId: user?.id,
            platformId: 0,
            dailyLimit: '',
        },
    })

    useEffect(() => {
        if (goal) {
            setValue('platformId', goal.platformId)
            setValue('dailyLimit', goal.dailyLimit)
        }
    }, [goal, setValue])

    const handleFormSubmit = useCallback(
        (data: UserUsageGoal) => {
            try {
                if (goal?.id) {
                    onSubmit({ ...data, id: goal.id })
                } else {
                    onSubmit(data)
                }
                reset()
                onClose()
            } catch (error) {
                console.error('Error updating goal:', error)
                Alert.alert('Error', 'Failed to update goal. Please try again.')
            }
        },
        [onSubmit, reset, onClose, goal]
    )

    const handleModalClose = useCallback(() => {
        reset()
        onClose()
    }, [reset, onClose])

    const dropdownData =
        platforms?.map((p) => ({
            label: p.name,
            value: p.id,
        })) || []

    return (
        <Modal
            isVisible={visible}
            onBackdropPress={handleModalClose}
            onBackButtonPress={handleModalClose}
            useNativeDriver
            style={styles.modal}
        >
            <SafeAreaView style={styles.modalContainer}>
                <View style={styles.container}>
                    <View style={styles.header}>
                        <Text style={styles.title}>Update Usage Goal</Text>
                        <TouchableOpacity
                            onPress={handleModalClose}
                            style={styles.closeButton}
                            disabled={isSubmitting}
                        >
                            <Text style={styles.closeButtonText}>×</Text>
                        </TouchableOpacity>
                    </View>

                    <View style={styles.content}>
                        <Text style={styles.label}>
                            Set Daily Limit (in minutes)
                        </Text>
                        <Controller
                            control={control}
                            name="dailyLimit"
                            render={({
                                field: { onChange, onBlur, value },
                            }) => (
                                <TextInput
                                    keyboardType="numbers-and-punctuation"
                                    onChangeText={(val) => onChange(val)}
                                    onBlur={onBlur}
                                    value={value ?? ''}
                                    style={styles.input}
                                    placeholder="Enter time (hh:mm:ss)"
                                />
                            )}
                        />
                        {errors.dailyLimit && (
                            <Text style={styles.error}>
                                {errors.dailyLimit.message}
                            </Text>
                        )}

                        <Text style={styles.label}>Select Platform</Text>
                        {!platforms || platforms.length === 0 ? (
                            <ActivityIndicator style={styles.loading} />
                        ) : (
                            <Controller
                                control={control}
                                name="platformId"
                                render={({ field: { value, onChange } }) => (
                                    <Dropdown
                                        style={styles.dropdown}
                                        placeholderStyle={
                                            styles.placeholderStyle
                                        }
                                        selectedTextStyle={
                                            styles.selectedTextStyle
                                        }
                                        data={dropdownData}
                                        maxHeight={300}
                                        labelField="label"
                                        valueField="value"
                                        placeholder="Select a platform"
                                        value={value === 0 ? null : value}
                                        onChange={(item) =>
                                            onChange(item.value)
                                        }
                                        disable={isSubmitting}
                                    />
                                )}
                            />
                        )}
                        {errors.platformId && (
                            <Text style={styles.error}>
                                {errors.platformId.message}
                            </Text>
                        )}
                    </View>

                    <View style={styles.buttonContainer}>
                        <TouchableOpacity
                            style={[
                                styles.primaryButton,
                                (isSubmitting || !platforms?.length) &&
                                    styles.disabledButton,
                            ]}
                            onPress={handleSubmit(handleFormSubmit)}
                            disabled={isSubmitting || !platforms?.length}
                        >
                            <Text style={styles.primaryButtonText}>
                                {isSubmitting ? 'Updating...' : 'Update Goal'}
                            </Text>
                        </TouchableOpacity>

                        <TouchableOpacity
                            style={[
                                styles.secondaryButton,
                                isSubmitting && styles.disabledButton,
                            ]}
                            onPress={handleModalClose}
                            disabled={isSubmitting}
                        >
                            <Text style={styles.secondaryButtonText}>
                                Cancel
                            </Text>
                        </TouchableOpacity>
                    </View>
                </View>
            </SafeAreaView>
        </Modal>
    )
}
