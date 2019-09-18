using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAffectedByWeight
{
    float weightOnObject { get; set; }
    void AddWeight(float weight);
    void RemoveWeight(float weight);
}
