#!/bin/bash
# For use with the JS - Block Disabler mod
# * Parses TypeId/SubtypeID from TSO Corridor mods in the steam workshop dir.
# * Removes all blocks that are not in the subtypes-keep.txt file.
# * Writes results to the DisabledBlocks.txt

# Steam Space Engineers workshop base directory
steam_ws_dir=/cygdrive/d/Steam/steamapps/workshop/content/244850
# Glob pattern to find TSO Corridor block sbc files within the Steam SE workshop directory
sbc_search_pattern="*/Data/CubeBlocks/TSOCor*.sbc"

# List of all TSO corridor block subtypes, in TypeId/SubtypeId format,
# as parsed from the TSO Corridor sbc files (output)
all_subtypes_file=subtypes-all.txt
# List of subtypes to keep (user input file)
keep_subtypes_file=subtypes-keep.txt
# List of blocks to disable (output)
disabled_blocks_file=Content/Data/DisabledBlocks.txt

for i in "${steam_ws_dir}"/${sbc_search_pattern}; do
  grep '[Tt]ypeId' "$i" | head -2 | sort -r | awk -F '[><]' '{print $3}' | paste - - | awk '{print $1"/"$2}'
done | sort > "${all_subtypes_file}"

diff "${all_subtypes_file}" "${keep_subtypes_file}" | awk '/</ {print $2}' > "${disabled_blocks_file}"
