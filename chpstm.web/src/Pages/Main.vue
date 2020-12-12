<template>
  <v-container style="margin-top:10px">
    <v-row align="start">
      <v-card  style="margin-bottom: 15px;width:100%">
          <v-card-title primary-title>
            <div>
              <h2 class="headline mb-0">Guide</h2>
            </div>
          </v-card-title>
          <v-card-text>
            <div id="guide">

            </div>
          </v-card-text>
        </v-card>
      <v-text-field style="margin-bottom: 15px" append-icon="search" solo placeholder="搜索饰品" hide-details="true"  @keydown="OnSearchKeyDown"
        ref="search_input" @click:append="SearchGoods" v-model="searchText"></v-text-field>
      <v-expansion-panels accordion style="margin-bottom: 15px">
        <v-expansion-panel>
          <v-expansion-panel-header> 筛选条件 </v-expansion-panel-header>
          <v-expansion-panel-content>
            <v-row align="start">
              <v-col lg="3" md="4" xs="6" sm="6">
                <v-checkbox label="STEAM 收购价" v-model="sorts" value="steam_buy_price"></v-checkbox>
              </v-col>
              <v-col lg="3" md="4" xs="6" sm="6">
                <v-checkbox label="STEAM 出售价" v-model="sorts" value="steam_sell_price"></v-checkbox>
              </v-col>
              <v-col lg="3" md="4" xs="6" sm="6">
                <v-checkbox label="STEAM 在售数" v-model="sorts" value="steam_sell_num"></v-checkbox>
              </v-col>
              <v-col lg="3" md="4" xs="6" sm="6">
                <v-checkbox label="BUFF 收购价" v-model="sorts" value="buff_buy_price"></v-checkbox>
              </v-col>
              <v-col lg="3" md="4" xs="6" sm="6">
                <v-checkbox label="BUFF 出售价" v-model="sorts" value="buff_sell_price"></v-checkbox>
              </v-col>
              <v-col lg="3" md="4" xs="6" sm="6">
                <v-checkbox label="BUFF 在售数" v-model="sorts" value="buff_sell_num"></v-checkbox>
              </v-col>
              <v-col lg="3" md="4" xs="6" sm="6">
                <v-checkbox label="STEAM 出售比例" v-model="sorts" value="steam_sell_radio"></v-checkbox>
              </v-col>
              <v-col lg="3" md="4" xs="6" sm="6">
                <v-checkbox label="STEAM 收购比例" v-model="sorts" value="steam_buy_radio"></v-checkbox>
              </v-col>
              <div style="width:100%">
                <v-text-field label="BUFF 最小售价" v-model="filtrationParams.min_buff_price"></v-text-field>
                <v-text-field label="BUFF 最大售价" v-model="filtrationParams.max_buff_price"></v-text-field>
                <v-text-field label="STEAM 最小售价" v-model="filtrationParams.min_steam_price"></v-text-field>
                <v-text-field label="STEAM 最大售价" v-model="filtrationParams.max_steam_price"></v-text-field>
                <v-text-field label="BUFF 最小在售数" v-model="filtrationParams.min_buff_sell_num"></v-text-field>
                <v-text-field label="STEAM 最小在售数" v-model="filtrationParams.min_steam_sell_num"></v-text-field>
                <v-select :items="game_kind_items" label="游戏类型" v-model="game_kind"></v-select>
                <v-select :items="order_items" label="以何值升序排序" v-model="orderValue"></v-select>

                <v-btn depressed color="primary" @click="RequreDatas(1)">
                  Apply
                </v-btn>
              </div>
            </v-row>
          </v-expansion-panel-content>
        </v-expansion-panel>
      </v-expansion-panels>
      <v-data-table :loading="isLoading" loading-text="少女祈祷中..." :headers="headers" class="elevation-1" :items="items"
        item-key="id" hide-default-footer :page.sync="page" style="width:100%" :items-per-page="itemsPerPage"
        ref="dataTable">
        <template v-slot:footer>
          <div class="text-center pt-2 pb-2">
            <v-pagination @input="TableGoNext" v-model="page" :length="pageCount" :total-visible="totalVisible" circle>
            </v-pagination>
          </div>
        </template>
      </v-data-table>
    </v-row>
    <v-btn class="mx-2" fab dark color="teal" style="position: fixed;bottom: 15px;right: 10px;z-index: 1;" @click="RequreDatas(page)">
      <v-icon dark>
        mdi-refresh
      </v-icon>
    </v-btn>
  </v-container>
</template>

<script>
  import axios from "axios";
  import {markdown} from 'markdown';
  export default {
    data() {
      return {
        order_items : [
          {
            text: "STEAM 收购比例",
            value: 0
          },
          {
            text : "STEAM 出售比例",
            value: 1
          }
        ],
        game_kind_items:[
          {
            text: "CSGO",
            value: 1
          },
          {
            text : "DOTA 2",
            value: 0
          }
        ],
        game_kind : 1,
        orderValue: 0,
        page: 1,
        pageCount: 1,
        searchText: "",
        sorts: [],
        headers: [],
        items: [],
        itemsPerPage: 30,
        totalVisible: 5,
        isLoading: false,
        filtrationParams: {
          min_buff_price: "10",
          max_buff_price: "200",
          min_steam_price: "12.5",
          max_steam_price: "250",
          min_buff_sell_num: "20",
          min_steam_sell_num: "25",
        },
      };
    },
    methods: {
      GetGuide : function ()
      {
        axios.get(`${process.env.VUE_APP_URL}guide.md`).then(result => {
          document.getElementById("guide").innerHTML = markdown.toHTML(result.data);
        })
      },
      OnSearchKeyDown : function (p)
      {
        if(p.code == "Enter")
        {
          this.SearchGoods();
        }
      },
      SearchGoods: function (params) {
        if(this.searchText != ""){
          this.isLoading = true;
        this.$vuetify.goTo(this.$refs.dataTable, "easeInOutCubic");
        axios.get(`${process.env.VUE_APP_URL}goods/search?name=${this.searchText}`).then(result => {
          this.UpdateDataTable(result);
        });
        } else {
          this.RequreDatas(1);
        }
        
      },
      TableGoNext: function (params) {
        this.RequreDatas(this.page);
      },
      UpdateDataTable: function (result) {
        this.items = [];
        this.pageCount = result.data['data']['pages_count'];
        result.data['data']['result'].forEach(item => {
          this.items.push({
            name: item['name'],
            buff_sell_num: item['buffSellNum'],
            steam_sell_num: item['steamSellNum'],
            steam_sell_price: item['steamSellPrice'],
            steam_buy_price: item['steamBuyPrice'],
            steam_sell_radio: item['steamSellRadio'],
            steam_buy_radio: item['steamBuyRadio'],
            date: (new Date(item['updateTime'])).toLocaleTimeString("en-US",{month:"numeric",day:"numeric",hour:"numeric",minute:"numeric"}),
            buff_buy_price: item['buffBuyPrice'],
            buff_sell_price: item['buffSellPrice'],
          })
        });
        this.isLoading = false;
      },
      RequreDatas: function (page) {
        this.isLoading = true;
        this.$vuetify.goTo(this.$refs.dataTable, "easeInOutCubic");
        axios.get(
          `${process.env.VUE_APP_URL}goods?min_buff_price=${this.filtrationParams.min_buff_price}&max_buff_price=${this.filtrationParams.max_buff_price}&min_buff_sell_num=${this.filtrationParams.min_buff_sell_num}&min_steam_sell_num=${this.filtrationParams.min_steam_sell_num}&min_steam_price=${this.filtrationParams.min_steam_price}&max_steam_price=${this.filtrationParams.max_steam_price}&page=${page}&order_by=${this.orderValue}&kind=${this.game_kind}`
          ).then(result => {
          this.UpdateDataTable(result);
        });
      }
    },
    mounted() {
      this.sorts = ["name", "buff_sell_num", "steam_sell_price", "buff_sell_price", "steam_sell_radio",
        "steam_buy_radio"
      ];
      this.RequreDatas(1);
      this.GetGuide();
    },
    watch: {
      sorts: function (newValue, oldalue) {
        var newHeader = [];
        newHeader.push({
          text: "名称",
          align: "start",
          sortable: false,
          value: "name",
        });
        newValue.forEach((element) => {
          switch (element) {
            case "steam_sell_num":
              newHeader.push({
                text: "STEAM 在售数",
                value: element,
              });
              break;
            case "buff_sell_num":
              newHeader.push({
                text: "BUFF 在售数",
                value: element,
              });
              break;
            case "steam_sell_price":
              newHeader.push({
                text: "STEAM 出售价",
                value: element,
              });
              break;
            case "steam_buy_price":
              newHeader.push({
                text: "STEAM 收购价",
                value: element,
              });
              break;
            case "buff_sell_price":
              newHeader.push({
                text: "BUFF 出售价",
                value: element,
              });
              break;
            case "buff_buy_price":
              newHeader.push({
                text: "STEAM 收购价",
                value: element,
              });
              break;
            case "steam_sell_radio":
              newHeader.push({
                text: "STEAM 出售比例",
                value: element,
              });
              break;
            case "steam_buy_radio":
              newHeader.push({
                text: "STEAM 收购比例",
                value: element,
              });
              break;
            default:
              break;
          }
        });
        newHeader.push({
          text: "更新日期",
          value: "date",
        });
        this.headers = newHeader;
      },
    },
  };
</script>