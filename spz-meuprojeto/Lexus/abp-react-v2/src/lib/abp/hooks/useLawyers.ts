import { useQuery } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetLawyersInput {
  filter?: string;
  skipCount?: number;
  maxResultCount?: number;
}

export function useLawyers(input: GetLawyersInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10 } = input;

  return useQuery({
    queryKey: ["lawyers", filter, skipCount, maxResultCount],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/lawyer", {
        params: {
          filter,
          skipCount,
          maxResultCount,
        },
      });
      return response.data;
    },
  });
}

export function useLawyer(id: string) {
  return useQuery({
    queryKey: ["lawyer", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/lawyer/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}
