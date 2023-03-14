import 'package:flutter/material.dart';
import 'package:flutter/services.dart';   // SystemUiOverlayStyle
import 'package:flutter_bloc/flutter_bloc.dart';    // BlocProvider  BlocBuilder
import 'package:flutter_gen/gen_l10n/app_localizations.dart';

import 'app_state_bloc/app_state.dart';
import 'app_state_bloc/app_state_bloc.dart';
import 'home_page/stopwatch_home_page.dart';

// void main() {
//   runApp(const StopwatchApp());
// }
//
// class StopwatchApp extends StatelessWidget {
//   const StopwatchApp({super.key});
//
//   // This widget is the root of your application.
//   @override
//   Widget build(BuildContext context) {
//     return MaterialApp(
//       debugShowCheckedModeBanner: false,
//       home: const StopWatchHomePage(),
//     );
//   }
// }

void main() {
  runApp(
    BlocProvider<AppStateBloc>(
      create: (_) => AppStateBloc(appConfig: AppState.defaultConfig()),
      child: const StopwatchApp(),
    ),
  );
}

class StopwatchApp extends StatelessWidget {
  const StopwatchApp({Key? key}) : super(key: key);

  @override
  Widget build(BuildContext context) {
    SystemUiOverlayStyle overlayStyle = const SystemUiOverlayStyle(
        statusBarColor: Colors.transparent,
        statusBarBrightness: Brightness.light,
        statusBarIconBrightness: Brightness.dark);

    return BlocBuilder<AppStateBloc, AppState>(
      builder: (_, state) {
        return MaterialApp(
          localizationsDelegates: AppLocalizations.localizationsDelegates,
          supportedLocales: AppLocalizations.supportedLocales,
          locale: state.locale, // 指定语言
          debugShowCheckedModeBanner: false,
          theme: ThemeData(
              primaryColor: state.themeColor,
              appBarTheme: AppBarTheme(
                systemOverlayStyle: overlayStyle,
              )),
          home: const StopWatchHomePage(),
        );
      },
    );
  }
}


