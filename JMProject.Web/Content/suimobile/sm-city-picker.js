/*!
* =====================================================
* SUI Mobile - http://m.sui.taobao.org/
*
* =====================================================
*/
// jshint ignore: start
+function ($) {

    $.smConfig.rawCitiesData = [
    {
        "name": "吉林省",
        "sub": [
            {
                "name": "请选择",
                "sub": [

                ]
            },
            {
                "name": "吉林省",
                "sub": [
                    {
                        "name": "请选择"
                    },
                    {
                        "name": "吉林省"
                    }
                ],
                "type": 0
            },
            {
                "name": "长春市",
                "sub": [
                    {
                        "name": "请选择"
                    },
                    {
                        "name": "长春市"
                    },
                    {
                        "name": "九台区"
                    },
                    {
                        "name": "德惠市"
                    },
                    {
                        "name": "榆树市"
                    },
                    {
                        "name": "双阳市"
                    }
                ],
                "type": 0
            },
            {
                "name": "吉林市",
                "sub": [
                    {
                        "name": "请选择"
                    },
                    {
                        "name": "吉林市"
                    },
                    {
                        "name": "永吉县"
                    },
                    {
                        "name": "磐石市"
                    },
                    {
                        "name": "舒兰市"
                    },
                    {
                        "name": "蛟河市"
                    },
                    {
                        "name": "桦甸市"
                    },
                    {
                        "name": "昌邑区"
                    }
                ],
                "type": 0
            },
            {
                "name": "白城市",
                "sub": [
                    {
                        "name": "请选择"
                    },
                    {
                        "name": "镇赉县"
                    },
                    {
                        "name": "洮南市"
                    },
                    {
                        "name": "白城市"
                    },
                    {
                        "name": "大安市"
                    }
                ],
                "type": 0
            },
            {
                "name": "辽源市",
                "sub": [
                    {
                        "name": "请选择"
                    },
                    {
                        "name": "辽源市"
                    },
                    {
                        "name": "东丰县"
                    },
                    {
                        "name": "东辽县"
                    }
                ],
                "type": 0
            },
            {
                "name": "通化市",
                "sub": [
                    {
                        "name": "请选择"
                    },
                    {
                        "name": "通化县"
                    },
                    {
                        "name": "辉南县"
                    },
                    {
                        "name": "通化市"
                    },
                    {
                        "name": "柳河县"
                    },
                    {
                        "name": "梅河口市"
                    },
                    {
                        "name": "集安市"
                    }
                ],
                "type": 0
            },
            {
                "name": "松原市",
                "sub": [
                    {
                        "name": "请选择"
                    },
                    {
                        "name": "松原市"
                    },
                    {
                        "name": "宁江区"
                    },
                    {
                        "name": "乾安县"
                    },
                    {
                        "name": "扶余市"
                    },
                    {
                        "name": "长岭县"
                    },
                    {
                        "name": "前郭尔罗斯蒙古族自治县"
                    }
                ],
                "type": 0
            },
            {
                "name": "农安县",
                "sub": [
                    {
                        "name": "请选择"
                    },
                    {
                        "name": "农安县"
                    }
                ],
                "type": 0
            },
            {
                "name": "四平市",
                "sub": [
                    {
                        "name": "请选择"
                    },
                    {
                        "name": "伊通满族自治县"
                    },
                    {
                        "name": "梨树县"
                    },
                    {
                        "name": "公主岭市"
                    },
                    {
                        "name": "四平市"
                    },
                    {
                        "name": "双辽市"
                    }
                ],
                "type": 0
            },
            {
                "name": "延边州",
                "sub": [
                    {
                        "name": "请选择"
                    },
                    {
                        "name": "安图县"
                    },
                    {
                        "name": "珲春市"
                    },
                    {
                        "name": "延边州"
                    },
                    {
                        "name": "延吉市"
                    },
                    {
                        "name": "图们市"
                    },
                    {
                        "name": "敦化市"
                    },
                    {
                        "name": "长白朝鲜族自治县"
                    },
                    {
                        "name": "和龙市"
                    },
                    {
                        "name": "汪清县"
                    },
                    {
                        "name": "龙井市"
                    }
                ],
                "type": 0
            },
            {
                "name": "白山市",
                "sub": [
                    {
                        "name": "请选择"
                    },
                    {
                        "name": "抚松县"
                    },
                    {
                        "name": "靖宇县"
                    },
                    {
                        "name": "通榆县"
                    },
                    {
                        "name": "临江市"
                    },
                    {
                        "name": "江源区"
                    },
                    {
                        "name": "白山市"
                    }
                ],
                "type": 0
            }
        ],
        "type": 1
    }
];

} (Zepto);
// jshint ignore: end

/* jshint unused:false*/

+function ($) {
    "use strict";
    var format = function (data) {
        var result = [];
        for (var i = 0; i < data.length; i++) {
            var d = data[i];
            if (d.name === "请选择") continue;
            result.push(d.name);
        }
        if (result.length) return result;
        return [""];
    };

    var sub = function (data) {
        if (!data.sub) return [""];
        return format(data.sub);
    };

    var getCities = function (d) {
        for (var i = 0; i < raw.length; i++) {
            if (raw[i].name === d) return sub(raw[i]);
        }
        return [""];
    };

    var getDistricts = function (p, c) {
        for (var i = 0; i < raw.length; i++) {
            if (raw[i].name === p) {
                for (var j = 0; j < raw[i].sub.length; j++) {
                    if (raw[i].sub[j].name === c) {
                        return sub(raw[i].sub[j]);
                    }
                }
            }
        }
        return [""];
    };

    var raw = $.smConfig.rawCitiesData;
    var provinces = raw.map(function (d) {
        return d.name;
    });
    var initCities = sub(raw[0]);
    var initDistricts = [""];

    var currentProvince = provinces[0];
    var currentCity = initCities[0];
    var currentDistrict = initDistricts[0];

    var t;
    var defaults = {

        cssClass: "city-picker",
        rotateEffect: false,  //为了性能

        onChange: function (picker, values, displayValues) {
            var newProvince = picker.cols[0].value;
            var newCity;
            if (newProvince !== currentProvince) {
                // 如果Province变化，节流以提高reRender性能
                clearTimeout(t);

                t = setTimeout(function () {
                    var newCities = getCities(newProvince);
                    newCity = newCities[0];
                    var newDistricts = getDistricts(newProvince, newCity);
                    picker.cols[1].replaceValues(newCities);
                    picker.cols[2].replaceValues(newDistricts);
                    currentProvince = newProvince;
                    currentCity = newCity;
                    picker.updateValue();
                }, 200);
                return;
            }
            newCity = picker.cols[1].value;
            if (newCity !== currentCity) {
                picker.cols[2].replaceValues(getDistricts(newProvince, newCity));
                currentCity = newCity;
                picker.updateValue();
            }
        },

        cols: [
        {
            textAlign: 'center',
            values: provinces,
            cssClass: "col-province"
        },
        {
            textAlign: 'center',
            values: initCities,
            cssClass: "col-city"
        },
        {
            textAlign: 'center',
            values: initDistricts,
            cssClass: "col-district"
        }
        ],

        raw: []
    };

    $.fn.cityPicker = function (params) {
        return this.each(function () {
            if (!this) return;
            var p = $.extend(defaults, params);
            raw = p.raw;
            //计算value
            if (p.value) {
                $(this).val(p.value.join(' '));
            } else {
                var val = $(this).val();
                val && (p.value = val.split(' '));
            }

            if (p.value) {
                //p.value = val.split(" ");
                if (p.value[0]) {
                    currentProvince = p.value[0];
                    p.cols[1].values = getCities(p.value[0]);
                }
                if (p.value[1]) {
                    currentCity = p.value[1];
                    p.cols[2].values = getDistricts(p.value[0], p.value[1]);
                } else {
                    p.cols[2].values = getDistricts(p.value[0], p.cols[1].values[0]);
                }
                !p.value[2] && (p.value[2] = '');
                currentDistrict = p.value[2];
            }
            $(this).picker(p);
        });
    };

} (Zepto);
