using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class HudOverlayController : MonoBehaviour
{
    public static HudOverlayController Instance { get; private set; }

    public enum FeedType { Kill, Pickup, Event }

    [Header("Feed Settings")]
    public float feedEntryLifetime = 3.5f;
    public int maxFeedEntries = 5;

    [Header("Default Weapon Icon (fallback if none passed)")]
    public Sprite defaultWeaponIcon;

    VisualElement _root;
    VisualElement _feedContainer;
    VisualElement _weaponDisplay;
    VisualElement _ammoBarFill;
    Image _weaponIcon;
    Label _weaponName;
    Label _weaponAmmo;

    int _lastMaxAmmo = -1;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void OnEnable()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _feedContainer = _root.Q<VisualElement>("feed-container");
        _weaponDisplay = _root.Q<VisualElement>("weapon-display");
        _weaponIcon = _root.Q<Image>("weapon-icon");
        _weaponName = _root.Q<Label>("weapon-name");
        _weaponAmmo = _root.Q<Label>("weapon-ammo");
        _ammoBarFill = _root.Q<VisualElement>("ammo-bar-fill");

        if (defaultWeaponIcon != null) _weaponIcon.sprite = defaultWeaponIcon;
    }

    // ═══════════════════════════════════════════════════════════
    // FEATURE 1 — KILL / EVENT FEED
    // ═══════════════════════════════════════════════════════════

    public void AddFeedEvent(FeedType type, string message, string highlightWord = "")
    {
        if (_feedContainer == null) return;

        var entry = new Label();
        entry.AddToClassList("feed-entry");
        entry.AddToClassList("feed-entry--enter");
        entry.enableRichText = true;

        switch (type)
        {
            case FeedType.Kill:
                entry.AddToClassList("feed-entry--kill");
                entry.text = BuildHighlighted(message, highlightWord, "#e63c3c");
                break;
            case FeedType.Pickup:
                entry.AddToClassList("feed-entry--pickup");
                entry.text = BuildHighlighted(message, highlightWord, "#d4a017");
                break;
            default:
                entry.AddToClassList("feed-entry--event");
                entry.AddToClassList("feed-text-white");
                entry.text = message;
                break;
        }

        _feedContainer.Insert(0, entry);
        entry.schedule.Execute(() => entry.RemoveFromClassList("feed-entry--enter")).ExecuteLater(20);

        while (_feedContainer.childCount > maxFeedEntries)
            _feedContainer.RemoveAt(_feedContainer.childCount - 1);

        entry.schedule.Execute(() =>
        {
            entry.AddToClassList("feed-entry--exit");
            entry.schedule.Execute(() =>
            {
                if (entry.parent != null) entry.RemoveFromHierarchy();
            }).ExecuteLater(300);
        }).ExecuteLater((long)(feedEntryLifetime * 1000));
    }

    string BuildHighlighted(string message, string highlightWord, string hexColor)
    {
        if (string.IsNullOrEmpty(highlightWord) || !message.Contains(highlightWord))
            return message;
        return message.Replace(highlightWord, $"<color={hexColor}>{highlightWord}</color>");
    }

    // ═══════════════════════════════════════════════════════════
    // FEATURE 2 — CURRENT WEAPON DISPLAY
    // Uses the weapon's real sprite (weapon.currentWeaponSpr), not emoji
    // ═══════════════════════════════════════════════════════════

    /// <summary>
    /// Call this with the actual weapon ScriptableObject from your weaponpickup script.
    /// </summary>
    public void SetCurrentWeapon(weapon weap)
    {
        if (weap == null || _weaponName == null) return;

        _weaponName.text = weap.name.ToUpper();

        if (weap.currentWeaponSpr != null)
            _weaponIcon.sprite = weap.currentWeaponSpr;

        // weapon.cs has no ammo field yet — hide ammo row for now
        // If you add an ammo system later, call SetAmmo() separately
        SetAmmo(-1, -1);

        TriggerSwapGlow();
    }

    /// <summary>
    /// Overload — call with raw name + sprite directly if you don't have a weapon SO handy.
    /// </summary>
    public void SetCurrentWeapon(string weaponName, Sprite icon, int ammoCount = -1, int maxAmmo = -1)
    {
        if (_weaponName == null) return;

        _weaponName.text = weaponName.ToUpper();
        if (icon != null) _weaponIcon.sprite = icon;

        SetAmmo(ammoCount, maxAmmo);
        TriggerSwapGlow();
    }

    public void SetAmmo(int ammoCount, int maxAmmo)
    {
        if (_weaponAmmo == null) return;

        if (ammoCount < 0)
        {
            _weaponAmmo.parent.style.display = DisplayStyle.None;
            return;
        }

        _weaponAmmo.parent.style.display = DisplayStyle.Flex;
        _weaponAmmo.text = maxAmmo >= 0 ? $"{ammoCount}/{maxAmmo}" : ammoCount.ToString();

        if (maxAmmo > 0 && _ammoBarFill != null)
        {
            float pct = Mathf.Clamp01((float)ammoCount / maxAmmo) * 100f;
            _ammoBarFill.style.width = new Length(pct, LengthUnit.Percent);
            _ammoBarFill.style.backgroundColor = pct <= 25f
                ? new Color(0.9f, 0.25f, 0.25f)
                : new Color(0.298f, 0.686f, 0.314f);
        }
    }

    void TriggerSwapGlow()
    {
        if (_weaponDisplay == null) return;
        _weaponDisplay.AddToClassList("weapon-display--swap");
        _weaponDisplay.schedule.Execute(() =>
            _weaponDisplay.RemoveFromClassList("weapon-display--swap")
        ).ExecuteLater(400);
    }
}