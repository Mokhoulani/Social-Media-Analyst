package com.mokhoulani.ReactApp.services;

import android.app.AppOpsManager;
import android.app.Notification;
import android.app.NotificationChannel;
import android.app.NotificationManager;
import android.app.Service;
import android.app.usage.UsageEvents;
import android.app.usage.UsageStatsManager;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Build;
import android.os.IBinder;
import android.provider.Settings;
import android.util.Log;
import androidx.core.app.NotificationCompat;

import com.mokhoulani.ReactApp.R;

import java.util.HashMap;
import java.util.Map;
import java.util.Timer;
import java.util.TimerTask;

public class AppUsageService extends Service {

    private static final String TAG = "AppUsageService";
    private static final String CHANNEL_ID = "AppUsageServiceChannel";
    private static final int NOTIFICATION_ID = 1001;
    private static final int CHECK_INTERVAL = 3000;

    private Timer timer;
    private String currentTrackedApp = null;
    private long sessionStartTime = 0;
    private Map<String, String> trackedApps;

    @Override
    public void onCreate() {
        super.onCreate();
        Log.d(TAG, "Service created");
        // Only minimal setup here (no startForeground(), no startActivity())
    }

    private boolean hasUsageAccessPermission() {
        AppOpsManager appOps = (AppOpsManager) getSystemService(Context.APP_OPS_SERVICE);
        int mode = appOps.checkOpNoThrow(AppOpsManager.OPSTR_GET_USAGE_STATS,
                android.os.Process.myUid(), getPackageName());
        return mode == AppOpsManager.MODE_ALLOWED;
    }

    private void initializeTrackedApps() {
        trackedApps = new HashMap<>();
        trackedApps.put("com.facebook.katana", "Facebook");
        trackedApps.put("com.instagram.android", "Instagram");
        trackedApps.put("com.zhiliaoapp.musically", "TikTok");
        trackedApps.put("com.twitter.android", "Twitter");
        trackedApps.put("com.snapchat.android", "Snapchat");
        trackedApps.put("com.linkedin.android", "LinkedIn");
    }

    private void startUsageTracking() {
        timer = new Timer();
        timer.scheduleAtFixedRate(new TimerTask() {
            @Override
            public void run() {
                checkCurrentApp();
            }
        }, 0, CHECK_INTERVAL);
    }

    private void checkCurrentApp() {
        try {
            UsageStatsManager usageStatsManager = (UsageStatsManager) getSystemService(Context.USAGE_STATS_SERVICE);
            if (usageStatsManager == null) return;

            long currentTime = System.currentTimeMillis();
            long startTime = currentTime - 10000;

            UsageEvents usageEvents = usageStatsManager.queryEvents(startTime, currentTime);
            UsageEvents.Event event = new UsageEvents.Event();
            String lastForegroundApp = null;

            while (usageEvents.hasNextEvent()) {
                usageEvents.getNextEvent(event);
                if (event.getEventType() == UsageEvents.Event.MOVE_TO_FOREGROUND) {
                    lastForegroundApp = event.getPackageName();
                }
            }

            handleAppChange(lastForegroundApp);

        } catch (Exception e) {
            Log.e(TAG, "Error checking current app: " + e.getMessage());
        }
    }

    private void handleAppChange(String currentApp) {
        boolean isTrackedApp = trackedApps.containsKey(currentApp);
        boolean wasTrackingApp = currentTrackedApp != null;

        if (isTrackedApp && !currentApp.equals(currentTrackedApp)) {
            if (wasTrackingApp) endUsageSession();
            startUsageSession(currentApp);

        } else if (wasTrackingApp && !isTrackedApp) {
            endUsageSession();
        }
    }

    private void startUsageSession(String packageName) {
        currentTrackedApp = packageName;
        sessionStartTime = System.currentTimeMillis();
        String appName = trackedApps.get(packageName);
        Log.d(TAG, "Started tracking: " + appName);

        updateNotification("Tracking: " + appName);
    }

    private void endUsageSession() {
        if (currentTrackedApp == null) return;

        long sessionEndTime = System.currentTimeMillis();
        long sessionDuration = sessionEndTime - sessionStartTime;
        String appName = trackedApps.get(currentTrackedApp);

        Log.d(TAG, "Ended tracking: " + appName + " (Duration: " + (sessionDuration / 1000) + "s)");
        saveSessionToPreferences(appName, sessionStartTime, sessionEndTime);

        currentTrackedApp = null;
        sessionStartTime = 0;

        updateNotification("Monitoring social media usage");
    }

    private void saveSessionToPreferences(String appName, long startTime, long endTime) {
        SharedPreferences prefs = getSharedPreferences("usage_sessions", MODE_PRIVATE);
        SharedPreferences.Editor editor = prefs.edit();
        String key = appName + "_" + startTime;
        String value = "start=" + startTime + ", end=" + endTime + ", duration=" + (endTime - startTime);
        editor.putString(key, value);
        editor.apply();
    }

    private void createNotificationChannel() {
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            NotificationChannel channel = new NotificationChannel(
                    CHANNEL_ID,
                    "App Usage Tracking",
                    NotificationManager.IMPORTANCE_LOW
            );
            channel.setDescription("Tracks social media app usage");
            channel.setShowBadge(false);

            NotificationManager manager = getSystemService(NotificationManager.class);
            if (manager != null) {
                manager.createNotificationChannel(channel);
            }
        }
    }

    private Notification createNotification() {
        return new NotificationCompat.Builder(this, CHANNEL_ID)
                .setContentTitle("Social Media Tracker")
                .setContentText("Monitoring social media usage")
                .setSmallIcon(R.drawable.ic_notification)
                .setPriority(NotificationCompat.PRIORITY_LOW)
                .setOngoing(true)
                .build();
    }

    private void updateNotification(String text) {
        Notification notification = new NotificationCompat.Builder(this, CHANNEL_ID)
                .setContentTitle("Social Media Tracker")
                .setContentText(text)
                .setSmallIcon(R.drawable.ic_notification)
                .setPriority(NotificationCompat.PRIORITY_LOW)
                .setOngoing(true)
                .build();

        NotificationManager manager = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
        if (manager != null) {
            manager.notify(NOTIFICATION_ID, notification);
        }
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {
        Log.d(TAG, "Service started");

        if (!hasUsageAccessPermission()) {
            Log.w(TAG, "Usage access permission is NOT granted.");
            // Don't launch Settings here! Let React Native handle permission request UI.
            stopSelf();
            return START_NOT_STICKY;
        }

        initializeTrackedApps();
        createNotificationChannel();
        startForeground(NOTIFICATION_ID, createNotification());
        startUsageTracking();

        return START_STICKY;
    }

    @Override
    public void onDestroy() {
        super.onDestroy();
        Log.d(TAG, "Service destroyed");

        if (timer != null) {
            timer.cancel();
            timer = null;
        }

        if (currentTrackedApp != null) {
            endUsageSession();
        }
    }

    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }
}
