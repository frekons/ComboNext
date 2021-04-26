using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ComboNext : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField]
    private TextMeshProUGUI _textObject;

    [Header("Properties")]
    public List<ComboNextMember> Members;

    public ComboNextCallback OnMemberChange;

    #region Fields

    [SerializeField]
    private int _currentIndex = 0;

    #endregion

    #region Properties

    public int CurrentIndex { get => _currentIndex; set => _currentIndex = value; }

    #endregion

    #region Unity Functions
    private void Awake()
    {
        if (_textObject == null)
        {
            Debug.LogError(nameof(_textObject) + " is null! Assign it from inspector.");
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        /*  */
        if (Members.Count > 0)
        {
            /* validating user input in inspector */
            CurrentIndex = MathMod(CurrentIndex, Members.Count);
        }

        /* updating UI */
        SetUI();
    }
#endif
    #endregion

    #region Util Functions
    /* https://stackoverflow.com/a/2691042 */
    static int MathMod(int a, int b)
    {
        return (Math.Abs(a * b) + a) % b;
    }
    #endregion

    /// <summary>
    /// ComboNext switches as direction value. Example: 1 -> go right 1 | -1 -> go left 1
    /// </summary>
    /// <param name="direction">1 -> go right | -1 -> go left</param>
    public void ChangeMember(int direction)
    {
        if (Members.Count <= 0)
            return;

        CurrentIndex = MathMod(CurrentIndex + direction, Members.Count);

        SetUI();

        OnMemberChange.Invoke(CurrentIndex);
    }

    /// <summary>
    /// Finds memberName in Member list and set
    /// </summary>
    /// <param name="memberName"></param>
    /// <exception cref="System.ArgumentException">Thrown when memberName is not found in Members</exception>
    public void ChangeMember(string memberName)
    {
        int index = Members.FindIndex(x => x.Text == memberName);

        if (index == -1)
        {
            throw new ArgumentException("Did not find a member with this name", nameof(memberName));
        }

        CurrentIndex = index;

        SetUI();

        OnMemberChange.Invoke(CurrentIndex);
    }

    /// <summary>
    /// Sets current index of ComboNext
    /// </summary>
    /// <param name="index">starts from 0</param>
    /// <exception cref="System.ArgumentOutOfRangeException">Thrown when index is out of range</exception>
    public void SetCurrentIndex(int index)
    {
        if (index < 0 || index >= Members.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }

        CurrentIndex = index;

        SetUI();

        OnMemberChange.Invoke(CurrentIndex);
    }

    /// <summary>
    /// Updates UI
    /// </summary>
    public void SetUI()
    {
        /* when Members is empty */
        if (Members.Count <= 0)
        {
            _textObject.text = string.Empty;

            return;
        }
            

        _textObject.text = Members[CurrentIndex].Text;
    }

    #region Subclasses

    [System.Serializable]
    public class ComboNextCallback : UnityEvent<int> /* argument is the index of selected one */
    {

    }

    [System.Serializable]
    public class ComboNextMember
    {
        public string Text;
    }

    #endregion
}
