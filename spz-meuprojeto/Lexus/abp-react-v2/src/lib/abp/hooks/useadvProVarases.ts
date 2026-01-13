import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProVarasesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProVarases(input: GetAdvProVarasesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProVarases", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-varas", {
        params: {
          filter,
          skipCount,
          maxResultCount,
          ...rest,
        },
      });
      return response.data;
    },
  });
}

export function useAllAdvProVarases() {
  return useQuery({
    queryKey: ["advProVarases", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-varas", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProVaras(id: string) {
  return useQuery({
    queryKey: ["advProVaras", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pro-varas/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProVaras() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pro-varas", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProVarases"] });
    },
  });
}

export function useUpdateAdvProVaras() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pro-varas/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProVarases"] });
      queryClient.invalidateQueries({ queryKey: ["advProVaras", data.id] });
    },
  });
}

export function useDeleteAdvProVaras() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pro-varas/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProVarases"] });
    },
  });
}


