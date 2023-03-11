import 'package:flutter/material.dart';

void main() {
  runApp(const StopwatchApp());
}

class StopwatchApp extends StatelessWidget {
  const StopwatchApp({super.key});

  // This widget is the root of your application.
  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      home: const MyHomePage(),
    );
  }
}

class MyHomePage extends StatefulWidget {
  const MyHomePage({super.key});

  // This widget is the home page of your application. It is stateful, meaning
  // that it has a State object (defined below) that contains fields that affect
  // how it looks.

  // This class is the configuration for the state. It holds the values (in this
  // case the title) provided by the parent (in this case the App widget) and
  // used by the build method of the State. Fields in a Widget subclass are
  // always marked "final".


  @override
  State<MyHomePage> createState() => _MyHomePageState();
}

class _MyHomePageState extends State<MyHomePage> {
  int _counter = 0;

  void _incrementCounter() {
    setState(() {
      // This call to setState tells the Flutter framework that something has
      // changed in this State, which causes it to rerun the build method below
      // so that the display can reflect the updated values. If we changed
      // _counter without calling setState(), then the build method would not be
      // called again, and so nothing would appear to happen.
      _counter++;
    });
  }

  @override
  Widget build(BuildContext context) {
    // This method is rerun every time setState is called, for instance as done
    // by the _incrementCounter method above.
    //
    // The Flutter framework has been optimized to make rerunning build methods
    // fast, so that you can just rebuild anything that needs updating rather
    // than having to individually change instances of widgets.
    return Scaffold(
      appBar: AppBar(
        backgroundColor: Colors.transparent,
        elevation: 0,
        actions: buildActions(),
      ),
      body: Column(
        children: [
          buildStopwatchPanel(),
          buildRecordPanel(),
          buildTools()
        ],
      ),
    );
  }

  //region 布局的三块区域
  Widget buildStopwatchPanel(){
    // MediaQuery.of(context).size 可以获取屏幕的尺寸
    double radius = MediaQuery.of(context).size.width/2*0.75;
    return Container(
      height: radius*2,
      color: Colors.blue,
    );
  }

  Widget buildRecordPanel(){
    return Expanded(
      child: Container(
        color: Colors.red,
      ),
    );
  }

  Widget buildTools(){
    return Container(
      height: 80,
      color: Colors.yellow,
    );
  }
  //endregion

  //region 顶部的三个按钮，小下拉显示“设置”
  List<Widget> buildActions(){
    return [
      PopupMenuButton<String>(
        itemBuilder: _buildItem,
        onSelected: _onSelectItem,
        icon: const Icon( Icons.more_vert_outlined, color: Color(0xff262626)),
        position:PopupMenuPosition.under,
        shape: const RoundedRectangleBorder(
            borderRadius: BorderRadius.all(Radius.circular(10))
        ),
      )
    ];
  }

  List<PopupMenuEntry<String>> _buildItem(BuildContext context) {
    return const [
      PopupMenuItem<String>(
        value: "设置",
        child: Center(child: Text("设置")),
      )
    ];
  }

  void _onSelectItem(String value) {

  }
//endregion

}
