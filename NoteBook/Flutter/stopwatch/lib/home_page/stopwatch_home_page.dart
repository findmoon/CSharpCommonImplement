import 'package:flutter/material.dart';
import 'package:flutter/scheduler.dart';

import 'widgets/stopwatch_widget.dart';
import 'widgets/button_tools.dart';
import 'widgets/record_panel.dart';
import '../routes/rtl_route.dart';
import '../setting_page/setting_page.dart';
import 'models/stopwatch_type.dart';
import 'models/time_record.dart';

class StopWatchHomePage extends StatefulWidget {
  const StopWatchHomePage({super.key});

  // This widget is the home page of your application. It is stateful, meaning
  // that it has a State object (defined below) that contains fields that affect
  // how it looks.

  // This class is the configuration for the state. It holds the values (in this
  // case the title) provided by the parent (in this case the App widget) and
  // used by the build method of the State. Fields in a Widget subclass are
  // always marked "final".


  @override
  State<StopWatchHomePage> createState() => _StopWatchHomePageState();
}

class _StopWatchHomePageState extends State<StopWatchHomePage> {
  StopWatchType _type = StopWatchType.none;

  Duration _duration =  Duration.zero;
  Duration _secondDuration = Duration.zero;
  late Ticker _ticker;
  List<TimeRecord> _records = [];

  @override
  void initState() {
    super.initState();
    _ticker = Ticker(_onTick);
  }

  Duration dt = Duration.zero;
  Duration lastDuration = Duration.zero;

  void _onTick(Duration elapsed) {
    setState(() {
      dt = elapsed - lastDuration;
      _duration += dt;
      if(_records.isNotEmpty){
        _secondDuration = _duration - _records.last.record;
      }
      lastDuration = elapsed;
    });
  }

  @override
  void dispose() {
    _ticker.dispose();
    super.dispose();
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
        actions: buildActions(), // 标题栏右侧显示
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
    return StopwatchWidget( // 使用 StopwatchWidget
      radius: radius,
      duration: _duration,
      secondDuration: _secondDuration,
    );
    // return Container(
    //   height: radius*2,
    //   color: Colors.blue,
    // );
  }

  Widget buildRecordPanel(){
    return Expanded(
      // child: Container(
      //   color: Colors.red,
      // ),
      child: RecordPanel(
         records: _records ,
      ),
    );
  }

  Widget buildTools(){
    return ButtonTools(
      state: _type,
      onRecoder: onRecoder,
      onReset: onReset,
      toggle: toggle,
    );
    // return Container(
    //   height: 80,
    //   color: Colors.yellow,
    // );
  }


  void onReset() {
    setState(() {
      _duration = Duration.zero;
      _secondDuration = Duration.zero;
      _type = StopWatchType.none;
      _records.clear(); // 清空记录
    });
  }

  void onRecoder() {
    Duration current = _duration;
    Duration addition = _duration;
    if(_records.isNotEmpty){
      addition = _duration - _records.last.record;
    }
    setState(() {
      _records.add(TimeRecord(record: current, addition: addition));
    });
  }

  void toggle() {
    bool running = _type == StopWatchType.running;
    if(running){
      _ticker.stop();
      lastDuration = Duration.zero;
    }else{
      _ticker.start();
    }
    setState(() {
      _type = running ? StopWatchType.stopped : StopWatchType.running;
    });
  }
  //endregion

  //region 顶部的三个按钮，弹出显示“设置”
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
    if(value == "设置"){
      // MaterialPageRoute 为 渐变过渡
      // Navigator.of(context).push(MaterialPageRoute(builder: (_)=> const SettingPage()));

      // 自定义 PageRouteBuilder 更改跳转的过渡效果
      // 从右向左过渡
      Navigator.of(context).push(Right2LeftRoute(child: const SettingPage()));
    }
  }
//endregion

}
