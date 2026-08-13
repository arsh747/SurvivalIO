// MainMenuGenerator.cs
// Assets/Scripts/ mein rakho
// Main Menu scene mein empty GameObject pe attach karo → Play karo

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuGenerator : MonoBehaviour
{
    public string gameSceneName = "Lvl1";

    // Reference: 1280x600 landscape (Note10+ landscape ~1280x600 safe area)
    const float RW = 1280f;
    const float RH = 600f;

    void Start()
    {
        if (GameObject.Find("MM_Canvas") != null) return;
        BuildUI();
    }

    // ─────────────────────────────────────────────────
    void BuildUI()
    {
        // ── CANVAS ──
        var canvasGO = new GameObject("MM_Canvas");
        var canvas   = canvasGO.AddComponent<Canvas>();
        canvas.renderMode  = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 10;

        var sc = canvasGO.AddComponent<CanvasScaler>();
        sc.uiScaleMode          = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        sc.referenceResolution  = new Vector2(RW, RH);
        sc.screenMatchMode      = CanvasScaler.ScreenMatchMode.Expand;
        canvasGO.AddComponent<GraphicRaycaster>();
        var C = canvasGO.transform;

        // ── BACKGROUND ──
        Box("BG", C, 0,0, RW,RH, Hex("0d1f0e"), true);

        // dark patches
        Box("P1",C,-455, 95,190, 85,new Color(.04f,.11f,.04f,.55f),true);
        Box("P2",C, 375,-80,230,100,new Color(.04f,.11f,.04f,.50f),true);
        Box("P3",C,   0,-185,290, 75,new Color(.04f,.11f,.04f,.45f),true);

        // grid
        Box("GH1",C,0, 100,RW,1,new Color(.1f,.29f,.1f,.30f),true);
        Box("GH2",C,0,-100,RW,1,new Color(.1f,.29f,.1f,.30f),true);
        Box("GV1",C,-210,0, 1,RH,new Color(.1f,.29f,.1f,.30f),true);
        Box("GV2",C, 210,0, 1,RH,new Color(.1f,.29f,.1f,.30f),true);

        // trail
        Box("Trail",C,0,0,500,1,new Color(.8f,.13f,0f,.18f),true);

        // overlay
        Box("OV",C,0,0,RW,RH,new Color(0,0,0,.58f),true);

        // ── TOP BAR ──
        Bar("TopBar",C, top:true);
        Label("TopL",C,"DEFENDER-IO  //  MAIN MENU",  7, Hex("2a5a2a"), -500, 285, 350,22, ALeft);
        Label("TopR",C,"AUDIO ON  |  <color=#cc2200>v0.1.0 ALPHA</color>", 7, Hex("2a5a2a"), 450,285,300,22, ARight, rich:true);

        // ── BOTTOM BAR ──
        Bar("BotBar",C, top:false);
        // pulsing dot + text
        var dot = Box("Dot",C,-610,-285, 7,7, Hex("4aaa4a"),true);
        dot.AddComponent<PulseDot>();
        Label("BotL", C,"SYSTEM ONLINE",              7,Hex("2a5a2a"),-545,-285,160,20,ALeft);
        Label("BotM", C,"SAMSUNG GALAXY NOTE10+ 5G  |  ANDROID 14",7,Hex("2a5a2a"),0,-285,440,20,ACenter);
        Label("BotR", C,"LANDSCAPE MODE",             7,Hex("1a5a1a"), 545,-285,180,20,ARight);

        // ── CORNER BRACKETS ──
        Bracket("TL",C,-621, 258, false,false);
        Bracket("TR",C, 621, 258, true, false);
        Bracket("BL",C,-621,-258, false,true);
        Bracket("BR",C, 621,-258, true, true);

        // ── LEFT STAT PANEL ──
        // 3 cards stacked, left side  x≈-490
        StatCard("S1",C,-490, 95,"BEST SCORE","24,880",0.72f,Hex("cc2200"));
        StatCard("S2",C,-490,  0,"TOTAL KILLS","1,342", 0.55f,Hex("4aaa4a"));
        StatCard("S3",C,-490,-95,"CURRENT LVL","LVL 4", 0.88f,Hex("cc7a00"));

        // ── RIGHT INTEL PANEL ──
        // Intel card  x≈490
        IntelCard("IC",C, 490, 40);
        // Controls card
        CtrlCard("CC",C, 490,-95);

        // ── CENTER ── eyebrow, title, subtitle, divider, buttons
        // eyebrow badge
        Box("EyeBG",C,0,175,150,17,new Color(.03f,.09f,.03f,.7f),true)
            .AddComponent<Outline>().effectColor = Hex("1a4a1a");
        Label("Eye",C,"TACTICAL SURVIVAL",7,Hex("3a8a3a"),0,175,148,17,ACenter,ls:3f);

        // title
        Label("Title",C,"DEFEND<color=#cc2200>.</color>IO",
              38,Hex("e8f5e8"),0,110,600,55,ACenter,bold:true,rich:true,ls:1f);

        // subtitle
        Label("Sub",C,"ZONE CONTROL  *  SURVIVE  *  DOMINATE",
              7,Hex("3a7a3a"),0,70,450,18,ACenter,ls:2f);

        // red divider
        Box("Div",C,0,48,180,1,new Color(.8f,.13f,0f,.8f),true);

        // ── BUTTONS ── (4 buttons, centered, fixed height 28, gap 33)
        // all on a single panel for safety
        float bY = 20f;
        BtnRow("Play",  C,"PLAY NOW",    bY,      Hex("cc220030"),Hex("cc2200"),Hex("ff6644"));
        BtnRow("Set",   C,"SETTINGS",    bY-34f,  Hex("1a4a1a18"),Hex("2a5a2a"),Hex("6ab86a"));
        BtnRow("LB",    C,"LEADERBOARD", bY-68f,  Hex("1a4a1a18"),Hex("2a5a2a"),Hex("6ab86a"));
        BtnRow("Quit",  C,"QUIT",        bY-102f, new Color(0,0,0,0),Hex("152515"),Hex("2a5a2a"));

        // hook up actions
        GameObject.Find("MM_BtnPlay") .GetComponent<Button>().onClick.AddListener(PlayGame);
        GameObject.Find("MM_BtnSet")  .GetComponent<Button>().onClick.AddListener(()=>Debug.Log("Settings"));
        GameObject.Find("MM_BtnLB")   .GetComponent<Button>().onClick.AddListener(()=>Debug.Log("Leaderboard"));
        GameObject.Find("MM_BtnQuit") .GetComponent<Button>().onClick.AddListener(QuitGame);

        Debug.Log("✅ MainMenu UI built!");
    }

    // ─────────────────────────────────────────────────
    //  COMPOUND WIDGETS
    // ─────────────────────────────────────────────────

    void StatCard(string id, Transform C, float x, float y,
                  string lbl, string val, float fill, Color fillCol)
    {
        float W=195f, H=70f;
        var card = Box("Stat"+id,C,x,y,W,H,new Color(.04f,.11f,.04f,.88f),true);
        var ol   = card.AddComponent<Outline>();
        ol.effectColor    = Hex("1a4a1a");
        ol.effectDistance = new Vector2(1,-1);

        // left accent
        Box("Stat"+id+"Acc",C, x-W/2f+1.5f, y, 3, H-6, Hex("2a7a2a"),true);

        Label("Stat"+id+"L", C, lbl, 6, Hex("2a6a2a"), x+3,y+22, W-10,12, ALeft, ls:2f);
        Label("Stat"+id+"V", C, val,17, Hex("6ad86a"),  x+3,y+4,  W-10,24, ALeft, bold:true);

        // bar bg + fill
        Box("Stat"+id+"BB",C, x+3,y-22, W-14,2, Hex("0f2a0f"),true);
        float fw = (W-14f)*fill;
        Box("Stat"+id+"BF",C, x+3-(W-14f-fw)/2f, y-22, fw,2, fillCol,true);
    }

    void IntelCard(string id, Transform C, float x, float y)
    {
        float W=115f, H=95f;
        var card = Box("Intel"+id,C,x,y,W,H,new Color(.04f,.11f,.04f,.88f),true);
        card.AddComponent<Outline>().effectColor = Hex("1a4a1a");

        Label("Intel"+id+"T",C,"INTEL FEED",6,Hex("2a6a2a"),x,y+37,W-10,12,ALeft,ls:2f);

        // items
        Label("Intel"+id+"1",C,"<color=#cc2200>*</color> Enemy active Zone 3",
              7,Hex("4a8a4a"),x,y+18,W-6,13,ALeft,rich:true);
        Label("Intel"+id+"2",C,"<color=#cc2200>*</color> Portal breach near Bldg",
              7,Hex("4a8a4a"),x,y+2, W-6,13,ALeft,rich:true);
        Label("Intel"+id+"3",C,"<color=#cc7a00>*</color> Low health — resupply!",
              7,Hex("cc7a00"),x,y-14,W-6,13,ALeft,rich:true);
    }

    void CtrlCard(string id, Transform C, float x, float y)
    {
        float W=115f, H=70f;
        var card = Box("Ctrl"+id,C,x,y,W,H,new Color(.04f,.11f,.04f,.88f),true);
        card.AddComponent<Outline>().effectColor = Hex("1a4a1a");

        Label("Ctrl"+id+"T",C,"CONTROLS",6,Hex("2a6a2a"),x,y+25,W-10,12,ALeft,ls:2f);

        CtrlRow("CR1",C,x,y+ 8,"MOVE", "JOYSTICK",W);
        CtrlRow("CR2",C,x,y- 7,"FIRE", "AUTO",    W);
        CtrlRow("CR3",C,x,y-22,"DODGE","SWIPE",   W);
    }

    void CtrlRow(string id,Transform C,float x,float y,string a,string k,float W)
    {
        Label(id+"A",C,a,7,Hex("3a6a3a"),x-W/2f+8,y,W/2f-4,12,ALeft);
        Label(id+"K",C,k,7,Hex("6aaa6a"),x+W/2f-8,y,W/2f-4,12,ARight);
    }

    void BtnRow(string id, Transform C, string lbl, float y,
                Color bg, Color border, Color tc)
    {
        float W=230f, H=27f;
        var go = new GameObject("MM_Btn"+id);
        go.transform.SetParent(C, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(.5f,.5f);
        rt.sizeDelta = new Vector2(W,H);
        rt.anchoredPosition = new Vector2(0,y);

        var img = go.AddComponent<Image>(); img.color = bg;
        var ol  = go.AddComponent<Outline>();
        ol.effectColor    = border;
        ol.effectDistance = new Vector2(1,-1);

        // left bar
        Box("MM_Btn"+id+"Bar", go.transform, -W/2f+2,0, 3,H-4, border, true);

        // label (full width, centered) — ONE TMP, no split
        var lblGO = new GameObject("MM_Btn"+id+"Lbl");
        lblGO.transform.SetParent(go.transform, false);
        var lRT = lblGO.AddComponent<RectTransform>();
        lRT.anchorMin = Vector2.zero; lRT.anchorMax = Vector2.one;
        lRT.offsetMin = new Vector2(10,0); lRT.offsetMax = new Vector2(-10,0);
        var t = lblGO.AddComponent<TextMeshProUGUI>();
        t.text = lbl; t.fontSize = 9; t.color = tc;
        t.alignment = TextAlignmentOptions.Midline;
        t.fontStyle = FontStyles.Bold;
        t.characterSpacing = 3f;
        t.enableWordWrapping = false;

        // right arrow
        var arrGO = new GameObject("MM_Btn"+id+"Arr");
        arrGO.transform.SetParent(go.transform,false);
        var aRT = arrGO.AddComponent<RectTransform>();
        aRT.anchorMin = new Vector2(1,0); aRT.anchorMax = new Vector2(1,1);
        aRT.sizeDelta = new Vector2(16,0); aRT.anchoredPosition = new Vector2(-9,0);
        var at = arrGO.AddComponent<TextMeshProUGUI>();
        at.text = ">"; at.fontSize = 9;
        at.color = new Color(tc.r,tc.g,tc.b,.6f);
        at.alignment = TextAlignmentOptions.Midline;

        var btn = go.AddComponent<Button>(); btn.targetGraphic = img;
        var cb = btn.colors;
        cb.normalColor      = bg;
        cb.highlightedColor = new Color(bg.r+.1f,bg.g+.08f,bg.b+.08f,.45f);
        cb.pressedColor     = new Color(Mathf.Max(0,bg.r-.05f),bg.g,bg.b,.7f);
        btn.colors = cb;
    }

    // ─────────────────────────────────────────────────
    //  PRIMITIVES
    // ─────────────────────────────────────────────────

    void Bar(string n, Transform C, bool top)
    {
        float sy = top ? 1f : 0f;
        var go  = new GameObject("MM_"+n); go.transform.SetParent(C,false);
        var rt  = go.AddComponent<RectTransform>();
        rt.anchorMin = new Vector2(0,sy); rt.anchorMax = new Vector2(1,sy);
        rt.pivot     = new Vector2(.5f,sy);
        rt.sizeDelta = new Vector2(0,22);
        rt.anchoredPosition = Vector2.zero;
        go.AddComponent<Image>().color = new Color(.016f,.031f,.016f,.92f);

        // border line
        var b = new GameObject("MM_"+n+"Line"); b.transform.SetParent(C,false);
        var br = b.AddComponent<RectTransform>();
        br.anchorMin = new Vector2(0,sy); br.anchorMax = new Vector2(1,sy);
        br.pivot     = new Vector2(.5f,sy);
        br.sizeDelta = new Vector2(0,1);
        br.anchoredPosition = new Vector2(0, top?-22f:22f);
        b.AddComponent<Image>().color = Hex("1a3a1a");
    }

    void Bracket(string id, Transform C, float x, float y, bool flipX, bool flipY)
    {
        float sx = flipX?-1:1, sy = flipY?-1:1;
        // horizontal arm
        var h = new GameObject("MM_Br"+id+"H"); h.transform.SetParent(C,false);
        var hR = h.AddComponent<RectTransform>();
        hR.anchorMin = hR.anchorMax = new Vector2(.5f,.5f);
        hR.sizeDelta = new Vector2(14,1.5f);
        hR.anchoredPosition = new Vector2(x+sx*7, y);
        h.AddComponent<Image>().color = Hex("2a6a2a");
        // vertical arm
        var v = new GameObject("MM_Br"+id+"V"); v.transform.SetParent(C,false);
        var vR = v.AddComponent<RectTransform>();
        vR.anchorMin = vR.anchorMax = new Vector2(.5f,.5f);
        vR.sizeDelta = new Vector2(1.5f,14);
        vR.anchoredPosition = new Vector2(x, y+sy*7);
        v.AddComponent<Image>().color = Hex("2a6a2a");
    }

    GameObject Box(string n, Transform p, float x,float y,float w,float h, Color col, bool asChild)
    {
        var go = new GameObject("MM_"+n);
        go.transform.SetParent(asChild?p:p, false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(.5f,.5f);
        rt.sizeDelta = new Vector2(w,h);
        rt.anchoredPosition = new Vector2(x,y);
        go.AddComponent<Image>().color = col;
        return go;
    }

    void Label(string n, Transform p, string txt, float sz, Color col,
               float x,float y,float w,float h,
               TextAlignmentOptions align,
               bool bold=false, bool rich=false,
               float ls=0f)
    {
        var go = new GameObject("MM_"+n); go.transform.SetParent(p,false);
        var rt = go.AddComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = new Vector2(.5f,.5f);
        rt.sizeDelta = new Vector2(w,h);
        rt.anchoredPosition = new Vector2(x,y);
        var t = go.AddComponent<TextMeshProUGUI>();
        t.text = txt; t.fontSize = sz; t.color = col;
        t.alignment = align;
        t.fontStyle = bold ? FontStyles.Bold : FontStyles.Normal;
        t.richText = rich;
        t.characterSpacing = ls;
        t.enableWordWrapping = false;
    }

    static TextAlignmentOptions ALeft   = TextAlignmentOptions.MidlineLeft;
    static TextAlignmentOptions ARight  = TextAlignmentOptions.MidlineRight;
    static TextAlignmentOptions ACenter = TextAlignmentOptions.Center;

    Color Hex(string h){ Color c; ColorUtility.TryParseHtmlString("#"+h,out c); return c; }

    public void PlayGame() => SceneManager.LoadScene(gameSceneName);
    public void QuitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

public class PulseDot : MonoBehaviour
{
    Image img; float t;
    void Start()  { img = GetComponent<Image>(); }
    void Update() { t += Time.deltaTime*1.8f; img.color=new Color(.29f,.67f,.29f,Mathf.PingPong(t,1f)); }
}