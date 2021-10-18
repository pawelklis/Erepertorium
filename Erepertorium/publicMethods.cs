using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


    public class publicMethods
    {

        public static string ShowTableFilter(string dgName, int filterNumber, bool popupfilters)
        {
            string s = @"
     <script src='../tablefilter/tablefilter.js'></script>
<script>

           var filtersConfig = {
            // instruct TableFilter location to import ressources from
            base_path: '../tablefilter/',
  

            alternate_rows: true,
            rows_counter: true,
            btn_reset: true,
            loader: false,
            mark_active_columns: true,
            highlight_keywords: false,
            no_results_message: true,
            popup_filters: " + popupfilters.ToString().ToLower() + @",

  
            custom_options: {
                sorts: [false]
            },

            extensions: [{ name: 'sort' }]


        };

        var tf" + filterNumber + " = new TableFilter('" + dgName + @"', filtersConfig);
        tf" + filterNumber + @".init();
    </script>";
            return s;
        }



    }
