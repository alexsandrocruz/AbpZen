import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { apiClient } from "../api-client";

interface GetAdvProMeritosesInput {
  filter ?: string;
  skipCount ?: number;
  maxResultCount ?: number;
  }

export function useAdvProMeritoses(input: GetAdvProMeritosesInput = {}) {
  const { filter, skipCount = 0, maxResultCount = 10, ...rest } = input;

  return useQuery({
    queryKey: ["advProMeritoses", filter, skipCount, maxResultCount, rest],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-meritos", {
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

export function useAllAdvProMeritoses() {
  return useQuery({
    queryKey: ["advProMeritoses", "all"],
    queryFn: async () => {
      const response = await apiClient.get("/api/app/adv-pro-meritos", {
        params: {
          maxResultCount: 1000,
        },
      });
      return response.data;
    },
  });
}

export function useAdvProMeritos(id: string) {
  return useQuery({
    queryKey: ["advProMeritos", id],
    queryFn: async () => {
      const response = await apiClient.get(`/api/app/adv-pro-meritos/${id}`);
      return response.data;
    },
    enabled: !!id,
  });
}

export function useCreateAdvProMeritos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (data: any) => {
      const response = await apiClient.post("/api/app/adv-pro-meritos", data);
      return response.data;
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProMeritoses"] });
    },
  });
}

export function useUpdateAdvProMeritos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async ({ id, data }: { id: string; data: any }) => {
      const response = await apiClient.put(`/api/app/adv-pro-meritos/${id}`, data);
      return response.data;
    },
    onSuccess: (data) => {
      queryClient.invalidateQueries({ queryKey: ["advProMeritoses"] });
      queryClient.invalidateQueries({ queryKey: ["advProMeritos", data.id] });
    },
  });
}

export function useDeleteAdvProMeritos() {
  const queryClient = useQueryClient();
  return useMutation({
    mutationFn: async (id: string) => {
      await apiClient.delete(`/api/app/adv-pro-meritos/${id}`);
    },
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["advProMeritoses"] });
    },
  });
}


