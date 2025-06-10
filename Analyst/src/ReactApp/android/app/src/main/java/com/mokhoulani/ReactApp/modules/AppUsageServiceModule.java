package com.mokhoulani.ReactApp.modules;

import android.app.AppOpsManager;
import android.content.Intent;
import android.os.Build;
import android.provider.Settings;
import android.util.Log;

import com.facebook.react.bridge.Promise;
import com.facebook.react.bridge.ReactApplicationContext;
import com.facebook.react.bridge.ReactContextBaseJavaModule;
import com.facebook.react.bridge.ReactMethod;
import com.mokhoulani.ReactApp.services.AppUsageService;

public class AppUsageServiceModule extends ReactContextBaseJavaModule {

    public AppUsageServiceModule(ReactApplicationContext context) {
        super(context);
    }

    @Override
    public String getName() {
        return "AppUsageService";
    }

    @ReactMethod
    public void startService() {
        ReactApplicationContext context = getReactApplicationContext();
        Intent intent = new Intent(context, AppUsageService.class);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.O) {
            context.startForegroundService(intent);
        } else {
            context.startService(intent);
        }
    }

    @ReactMethod
    public void stopService() {
        ReactApplicationContext context = getReactApplicationContext();
        Intent intent = new Intent(context, AppUsageService.class);
        context.stopService(intent);
    }

    @ReactMethod
    public void openUsageAccessSettings() {
        Intent intent = new Intent(Settings.ACTION_USAGE_ACCESS_SETTINGS);
        intent.addFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        getReactApplicationContext().startActivity(intent);
    }

    @ReactMethod
    public void hasUsagePermission(Promise promise) {
        AppOpsManager appOps = (AppOpsManager) getReactApplicationContext().getSystemService(android.content.Context.APP_OPS_SERVICE);
        int mode = appOps.checkOpNoThrow(AppOpsManager.OPSTR_GET_USAGE_STATS,
                android.os.Process.myUid(), getReactApplicationContext().getPackageName());
        promise.resolve(mode == AppOpsManager.MODE_ALLOWED);
    }
}
