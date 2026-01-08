import { useQuery } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetCasesInput {
  filter?: string;
  skipCount?: number;
  maxResultCount?: number;
}

export function useCases(input: GetCasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10 } = input;

  return useQuery({
    queryKey: ["cases", filter, skipCount, maxResultCount],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/case", {
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

export function useCase(id: string) {
  return useQuery({
    queryKey: ["case", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/case/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}
