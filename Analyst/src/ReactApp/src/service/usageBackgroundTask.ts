// backgroundTask.ts
import * as BackgroundFetch from 'expo-background-fetch'
import * as TaskManager from 'expo-task-manager'

const TASK_NAME = 'background-fetch-task'

TaskManager.defineTask(TASK_NAME, async () => {
    console.log('🔄 Background task running...')

    try {
        // Your logic here
        // e.g., fetch usage data, save to local storage, send to server

        return BackgroundFetch.BackgroundFetchResult.NewData
    } catch (err) {
        console.error('❌ Background fetch failed:', err)
        return BackgroundFetch.BackgroundFetchResult.Failed
    }
})

export async function usageBackgroundTask() {
    const status = await BackgroundFetch.getStatusAsync()
    if (status === BackgroundFetch.BackgroundFetchStatus.Available) {
        await BackgroundFetch.registerTaskAsync(TASK_NAME, {
            minimumInterval: 15 * 60, // ⏱ minimum allowed by OS (~15 min)
            stopOnTerminate: false,
            startOnBoot: true,
        })
        console.log('✅ Background fetch task registered')
    } else {
        console.warn('⚠️ Background fetch unavailable:', status)
    }
}
